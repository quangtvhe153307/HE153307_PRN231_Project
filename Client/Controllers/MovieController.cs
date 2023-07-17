using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class MovieController : Controller
    {
        public IActionResult Index(int id, int episodeId)
        {
            return View();
        }
    }
}
