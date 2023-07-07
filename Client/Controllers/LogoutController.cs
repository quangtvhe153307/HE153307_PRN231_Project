using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class LogoutController : Controller
    {
        public IActionResult Index()
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(-7)
            };
            //Remove accesstoken
            if (Request.Cookies["accessToken"] != null) { 
                Response.Cookies.Delete("accessToken", cookieOptions); 
            }            
            //Remove refreshToken
            if (Request.Cookies["refreshToken"] != null) { 
                Response.Cookies.Delete("refreshToken", cookieOptions); 
            }
            return RedirectPermanent("/Login/Index");
        }
    }
}
