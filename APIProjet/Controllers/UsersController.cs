using APIProject.Controllers;
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
    [Authorize(Roles = "Administrator,VIP,Normal")]
    public class UsersController : ODataController
    {
        private IUserRepository _repository;
        private IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IJWTUtils _jwtUtils;
        private readonly IConfiguration _config;
        private readonly IRefreshtokenRepository _refreshtokenRepository;
        private readonly IAESUtils _aESUtils;
        private readonly ISendMailUtils _sendMailUtils;
        public UsersController(IMapper mapper, IJWTUtils jWTUtils, IUserRepository res, IConfiguration configuration, IRefreshtokenRepository refreshtokenRepository, IAESUtils aESUtils, ISendMailUtils sendMailUtils, IRoleRepository roleRepository)
        {
            _mapper = mapper;
            _jwtUtils = jWTUtils;
            _repository = res;
            _config = configuration;
            _refreshtokenRepository = refreshtokenRepository;
            _aESUtils = aESUtils;
            _sendMailUtils = sendMailUtils;
            _roleRepository = roleRepository;
        }
        [Authorize(Roles = "Administrator")]
        [EnableQuery(PageSize = 10)]
        public ActionResult<IQueryable<GetUserResponseDTO>> Get()
        {
            List<User> users = _repository.GetUsers();
            List<GetUserResponseDTO> getUserResponseDTOs = _mapper.Map<List<GetUserResponseDTO>>(users);
            return Ok(getUserResponseDTOs);
        }
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator,VIP,Normal")]
        [EnableQuery]
        [HttpGet("/Profile")]
        public ActionResult<GetUserResponseDTO> Details()
        {
            User user = _repository.GetUserById(LoggedUserId());
            user.PurchasedMovies = null;
            if (user == null)
            {
                return NotFound();
            }
            GetUserResponseDTO getUserResponseDTO = _mapper.Map<GetUserResponseDTO>(user);
            return Ok(getUserResponseDTO);
        }
        [Authorize(Roles = "Administrator")]
        [EnableQuery]
        public IActionResult Post([FromBody] CreateUserRequestDTO createUserRequestDTO)
        {
            User existUser = _repository.GetUserByEmail(createUserRequestDTO.Email);
            if (existUser != null)
            {
                return BadRequest(new { message = "User already exist!" });
            }
            User user = _mapper.Map<User>(createUserRequestDTO);
            _repository.SaveUser(user);

            GetUserResponseDTO responseDTO = _mapper.Map<GetUserResponseDTO>(user);
            return Created(responseDTO);
        }        
        [Authorize(Roles = "Administrator")]
        [EnableQuery]
        [HttpPost("/AddUser")]
        public IActionResult AddUser([FromBody] AddUserDTO createUserRequestDTO)
        {
            User existUser = _repository.GetUserByEmail(createUserRequestDTO.Email);
            if (existUser != null)
            {
                return BadRequest(new { message = "User already exist!" });
            }
            User user = _mapper.Map<User>(createUserRequestDTO);
            _repository.SaveUser(user);

            GetUserResponseDTO responseDTO = _mapper.Map<GetUserResponseDTO>(user);
            return Created(responseDTO);
        }
        [Authorize(Roles = "Administrator,VIP,Normal")]
        [HttpPost("/Upgrade")]
        public IActionResult Upgrade()
        {
            User user = _repository.GetUserById(LoggedUserId());
            if (user == null)
            {
                return NotFound();
            }
            if (user.RoleId == 2 || user.RoleId == 1)
            {
                return BadRequest(new { message ="You are already a premium user"});
            }
            if(user.Balance < 100)
            {
                return BadRequest(new { message = "Balance not enough" });
            }
            Role premiumRole = _roleRepository.GetRoleById(2);
            user.Role = premiumRole;
            user.RoleId = premiumRole.RoleId;
            user.Balance -= 100;
            user.ExpirationDate = DateTime.Today.AddMonths(1);
            _repository.UpdateUser(user);
            user.PurchasedMovies = null;
            GetUserResponseDTO responseDTO = _mapper.Map<GetUserResponseDTO>(user);
            return Ok(responseDTO);
        }        
        [Authorize(Roles = "Administrator,VIP,Normal")]
        [HttpPost("/ChangePassword")]
        public IActionResult ChangePassword([FromBody] UserChangePasswordRequestDTO model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            User user = _repository.GetUserById(LoggedUserId());
            if(!user.Password.Equals(model.Password))
            {
                return BadRequest(new { message = "Wrong password"});
            }
            user.Password = model.NewPassword;
            _repository.UpdateUser(user);
            user.PurchasedMovies = null;
            GetUserResponseDTO responseDTO = _mapper.Map<GetUserResponseDTO>(user);
            return Ok(responseDTO);
        }
        [Authorize(Roles = "Administrator")]
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
            tempUser.Email= user.Email;
            _repository.UpdateUser(tempUser);
            user.PurchasedMovies = null;
            return Updated(tempUser);
        }
        [Authorize(Roles = "Administrator")]
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
            if (!user.EmailConfirmed)
            {
                return BadRequest(new { message = "Your account is not verificated yet. Check your account to finish verification step!" });
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
            try
            {
                var oldRefreshToken = Request.Cookies["refreshToken"];
                var user = _repository.GetUserByRefreshToken(oldRefreshToken);
                if(user == null)
                {
                    return BadRequest();
                }
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
                Console.WriteLine(jwtToken);
                return Ok(authenticateResponse);
            } catch (Exception ex)
            {
                return BadRequest();
            }
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
        [AllowAnonymous]
        [HttpPost("/RegisterUser")]
        public IActionResult RegisterUser([FromBody] CreateUserRequestDTO createUserRequestDTO)
        {
            User existUser = _repository.GetUserByEmail(createUserRequestDTO.Email);
            if (existUser != null)
            {
                return BadRequest(new { message = "User already exist!" });
            }
            User user = _mapper.Map<User>(createUserRequestDTO);
            user.RoleId = 3;
            _repository.SaveUser(user);

            user = _repository.GetUserById(user.UserId);
            string token = _aESUtils.Encrypt(user);

            _sendMailUtils.SendAccountVerification(user.Email, user.UserId, token);
            return Ok(new { UserId = user.UserId, Token = token });
            //return Ok();
        }
        [AllowAnonymous]
        [HttpGet("/ConfirmEmail/{userId}/{token}")]
        public IActionResult ConfirmEmail(int userId, string token)
        {
            User userFromDB = _repository.GetUserById(userId);
            if (userFromDB == null)
            {
                return BadRequest(new { message = "Your link is wrong or encountered an error." });
            }
            token = Uri.UnescapeDataString(token);
            User user = _aESUtils.Decrypt(token);
            if (user.Equals(userFromDB))
            {
                Console.WriteLine("email confirmed");
                userFromDB.EmailConfirmed= true;
                _repository.UpdateUser(userFromDB);
                return Ok(new { message = "Verification successfully." });
            }
            else
            {
                return BadRequest(new { message = "Your link is wrong or encountered an error." });
            }
        }
        [AllowAnonymous]
        [HttpPost("/ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ForgotPasswordDTO model)
        {   
            if(!ModelState.IsValid)
            {
                return BadRequest(new { message = "Not valid" });
            }
            User user = _repository.GetUserByEmail(model.Email);
            if(user == null) {
                return BadRequest(new { message = "Your does not exist." });
            }
            try
            {
                user.Password = JWTUtils.GenerateNewPassword();
                //save user
                _repository.UpdateUser(user);
                await _sendMailUtils.SendMailResetPassword(user.Email, user.Password);
                return Ok(new { message = "Your new password has been sent to your email." });
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { message = "Error." });
            }
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
        private int LoggedUserId()
        {
            var userIdString = User.Claims.ToList()[4].Value;
            int userId = Int32.Parse(userIdString);
            return userId;
        }
    }
}
