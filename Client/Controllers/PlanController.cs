using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class PlanController : Controller
    {
        public IActionResult Upgrade()
        {
            return View();
        }
    }
}
