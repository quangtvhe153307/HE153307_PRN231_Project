using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class SignOutController : Controller
    {
        public IActionResult Index()
        {
            //Remove accesstoken
            if (Request.Cookies["accessToken"] != null) { 
                Response.Cookies.Delete("accessToken"); 
            }            
            //Remove refreshToken
            if (Request.Cookies["refreshToken"] != null) { 
                Response.Cookies.Delete("refreshToken"); 
            }
            return RedirectToAction("Index", "Login");
        }
    }
}
