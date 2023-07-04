using APIProject.DTO;
using APIProject.DTO.User;
using APIProject.Util;
using AutoMapper;
using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.UriParser;
using Repository.IRepository;
using Repository.Repository;
using System.Net;

namespace api.Controllers
{
    [Authorize]
    public class UsersController : ODataController
    {
        private IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly IJWTUtils _jwtUtils;
        private readonly IConfiguration _config;
        private readonly IRefreshtokenRepository _refreshtokenRepository;
        public UsersController(IMapper mapper, IJWTUtils jWTUtils, IUserRepository res, IConfiguration configuration, IRefreshtokenRepository refreshtokenRepository)
        {
            _mapper = mapper;
            _jwtUtils = jWTUtils;
            _repository = res;
            _config = configuration;
            _refreshtokenRepository = refreshtokenRepository;
        }
        [EnableQuery(PageSize = 10)]
        public ActionResult<IQueryable<GetUserResponseDTO>> Get()
        {
            List<User> users = _repository.GetUsers();
            List<GetUserResponseDTO> getUserResponseDTOs = _mapper.Map<List<GetUserResponseDTO>>(users);
            return Ok(getUserResponseDTOs);
        }
        [EnableQuery]
        public ActionResult<GetUserResponseDTO> Get([FromRoute] int key)
        {
            User user = _repository.GetUserById(key);
            if (user == null)
            {
                return NotFound();
            }
            GetUserResponseDTO getUserResponseDTO = _mapper.Map<GetUserResponseDTO>(user);
            return Ok(getUserResponseDTO);
        }
        [EnableQuery]
        public IActionResult Post([FromBody] CreateUserRequestDTO createUserRequestDTO)
        {
            User user = _mapper.Map<User>(createUserRequestDTO);
            _repository.SaveUser(user);

            GetUserResponseDTO responseDTO = _mapper.Map<GetUserResponseDTO>(user);
            return Created(responseDTO);
        }
        [EnableQuery]
        public ActionResult Put([FromRoute] int key, [FromBody] UpdateUserRequestDTO updateUserRequestDTO)
        {
            if (key != updateUserRequestDTO.UserId)
            {
                return BadRequest();
            }
            User tempUser = _repository.GetUserById(key);
            if (tempUser == null)
            {
                return NotFound();
            }
            User user = _mapper.Map<User>(updateUserRequestDTO);
            user.UserId = tempUser.UserId;
            _repository.UpdateUser(user);
            return Updated(user);
        }
        [EnableQuery]
        public ActionResult Delete([FromRoute] int key)
        {
            User tempUser = _repository.GetUserById(key);
            if (tempUser == null)
            {
                return NotFound();
            }
            _repository.DeleteUser(tempUser);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateRequest model)
        {
            var user = _repository.Authenticate(model.Email, model.Password);
            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            //Generate access Token and refresh Token
            var accessToken = _jwtUtils.GenerateJwtToken(user);
            var refreshToken = _jwtUtils.GenerateRefreshToken(IpAddress());

            //Save new genereated refreshtoken
            refreshToken.UserId = user.UserId;
            //_refreshtokenRepository.SaveRefreshtoken(refreshToken);

            //add new refresh token to user
            if (user.RefreshTokens == null)
            {
                user.RefreshTokens = new List<RefreshToken>
                {
                    refreshToken
                };
            }
            else
            {
                user.RefreshTokens.Add(refreshToken);
            }

            RemoveOldRefreshToken(user);

            //update user
            _repository.UpdateUser(user);

            //map to response
            AuthenticateResponse response = _mapper.Map<AuthenticateResponse>(user);
            response.AccessToken = accessToken;
            response.RefreshToken = refreshToken.Token;

            SetTokenCookie(response.RefreshToken);
            return Ok(response);
        }
        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public IActionResult RefreshToken()
        {
            var oldRefreshToken = Request.Cookies["refreshToken"];
            var user = _repository.GetUserByRefreshToken(oldRefreshToken);

            var refreshToken = user.RefreshTokens.Single(x => x.Token.Equals(oldRefreshToken));

            if (refreshToken.IsRevoked)
            {
                // revoke all descendant tokens in case this token has been compromised
                RevokeDescendantRefreshTokens(refreshToken, user, IpAddress(), $"Attempted reuse of revoked ancestor token: {oldRefreshToken}");
                _repository.UpdateUser(user);
            }

            if (!refreshToken.IsActive)
            {
                return BadRequest(new { message = "Invalid token" });
            }


            // replace old refresh token with a new one (rotate token)
            var newRefreshToken = RotateRefreshToken(refreshToken, IpAddress());
            user.RefreshTokens.Add(newRefreshToken);

            // remove old refresh tokens from user
            RemoveOldRefreshToken(user);

            // save changes to db
            _repository.UpdateUser(user);

            // generate new jwt
            var jwtToken = _jwtUtils.GenerateJwtToken(user);

            var authenticateResponse = new AuthenticateResponse
            {
                AccessToken = jwtToken,
                Email = user.Email,
                UserId = user.UserId,
                RefreshToken = newRefreshToken.Token
            };

            SetTokenCookie(newRefreshToken.Token);
            return Ok(authenticateResponse);
        }

        [HttpPost("revoke-token")]
        public IActionResult RevokeToken(RevokeTokenRequest model)
        {
            // accept refresh token in request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            var user = _repository.GetUserByRefreshToken(token);
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
                return BadRequest(new { message = "Invalid token" });

            // revoke token and save
            RevokeRefreshToken(refreshToken, IpAddress(), "Revoked without replacement");
            _repository.UpdateUser(user);
            return Ok(new { message = "Token revoked" });
        }
        private void SetTokenCookie(string token)
        {
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
        private string IpAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
        private void RemoveOldRefreshToken(User user)
        {
            // remove old inactive refresh tokens from user based on TTL in app settings
            user.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.Created.AddDays(double.Parse(_config["jwt:RefreshTokenTTL"])) <= DateTime.UtcNow);
        }
        private void RevokeDescendantRefreshTokens(RefreshToken refreshToken, User user, string ipAddress, string reason)
        {
            // recursively traverse the refresh token chain and ensure all descendants are revoked
            if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
            {
                var childToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
                if (childToken.IsActive)
                    RevokeRefreshToken(childToken, ipAddress, reason);
                else
                    RevokeDescendantRefreshTokens(childToken, user, ipAddress, reason);
            }
        }
        private void RevokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.ReasonRevoked = reason;
            token.ReplacedByToken = replacedByToken;
        }
        private RefreshToken RotateRefreshToken(RefreshToken refreshToken, string ipAddress)
        {
            var newRefreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
            RevokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
            return newRefreshToken;
        }
    }
}
