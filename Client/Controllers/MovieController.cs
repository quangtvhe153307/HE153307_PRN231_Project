using APIProject.DTO.Movie;
using BusinessObjects;
using Client.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class MovieController : Controller
    {
        public async Task<IActionResult> Index(int id, int episodeId)
        {
            
            try
            {
                var movie = await HttpUtils.GetObject<GetMovieResponseDTO>($"odata/Movies/{id}?$expand=Categories,MovieSeasons($expand=MovieEpisodes)");
                var episodeUrl = await HttpUtils.GetObject<string>("/MovieSource/"+episodeId);
                ViewData["url"] = episodeUrl;
                return View(movie);
            } catch {
            }

            return RedirectPermanent("/Home/Index");
        }
    }
}
