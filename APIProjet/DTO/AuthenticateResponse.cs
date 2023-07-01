using System.Text.Json.Serialization;

namespace APIProject.DTO
{
    public class AuthenticateResponse
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }
    }
}
