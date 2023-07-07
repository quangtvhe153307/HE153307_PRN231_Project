using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository.IRepository;
using Repository.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace APIProject.Util
{
    public interface IJWTUtils
    {
        public string GenerateJwtToken(User user);
        public RefreshToken GenerateRefreshToken(string ipAddress);
    }
    public class JWTUtils : IJWTUtils
    {
        private IConfiguration _config;
        private readonly IUserRepository repository;
        public JWTUtils(IConfiguration configuration, IUserRepository res)
        {
            _config = configuration;
            repository = res;
        }

        public string GenerateJwtToken(User user)
        {
            //create claims details based on the user information
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _config["jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim(ClaimTypes.Role, user.Role.RoleName),
                        new Claim("UserId", user.UserId.ToString()),
                        new Claim("Email", user.Email)
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["jwt:secret"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _config["jwt:issuer"],
                _config["jwt:audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public RefreshToken GenerateRefreshToken(string ipAddress)
        {
            var refreshToken = new RefreshToken
            {
                Token = getUniqueToken(),
                // token is valid for 7 days
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };

            return refreshToken;

            string getUniqueToken()
            {
                // token is a cryptographically strong random sequence of values
                var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

                // ensure token is unique by checking against db
                var tokenIsUnique = !repository.ContainRefreshToken(token);

                if (!tokenIsUnique)
                    return getUniqueToken();

                return token;
            }
        }

        public static void SetRefreshToken(string refreshToken, HttpResponse response)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
                IsEssential = true
            };
            response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
        public static void SetAccessToken(HttpResponse response, string accessToken)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(7),
                IsEssential = true
            };
            response.Cookies.Append("accessToken", accessToken, cookieOptions);
        }
        public static string GenerateNewPassword()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
        }
    }
}
