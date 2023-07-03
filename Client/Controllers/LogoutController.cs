using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class LogoutController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                HttpContext.Session.Remove("UserId");
            }
            return RedirectPermanent("/Login/Index");
        }
    }
}
