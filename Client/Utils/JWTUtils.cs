namespace Client.Utils
{
    public class JWTUtils
    {
        public static void SetRefreshToken(HttpResponseMessage responseMessage, HttpResponse response)
        {
            var refreshTokenCookie = responseMessage.Headers.GetValues("Set-Cookie");
            string cookie = Uri.UnescapeDataString(refreshTokenCookie.ToArray()[0]);
            int indexOfFirstSemiColon = cookie.IndexOf(";");
            int indexOfFirstEqual = cookie.IndexOf("=");

            string token = cookie.Substring(indexOfFirstEqual + 1, indexOfFirstSemiColon - indexOfFirstEqual - 1);

            Console.WriteLine("success");
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
                IsEssential = true
            };
            response.Cookies.Append("refreshToken", token, cookieOptions);
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
    }
}
