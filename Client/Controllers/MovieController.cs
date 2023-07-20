using APIProject.DTO.Comment;
using APIProject.DTO.Movie;
using BusinessObjects;
using Client.Models;
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
                var comment = await HttpUtils.GetObject<ODataReponseModel<GetCommentResponseDTO>>("/odata/Comments?$expand=User&$orderby=CommentedDate desc");
                ViewData["comment"] = comment;
                var episodeUrl = await HttpUtils.GetObject<string>("/MovieSource/"+episodeId);
                ViewData["url"] = episodeUrl;

            //https://localhost:7038/odata/Movies?$filter=Categories/any(c: c/CategoryId eq 1 or c/CategoryId eq 2)&$orderby=ViewCount desc
                return View(movie);
            } catch {
            }

            return RedirectPermanent("/Home/Index");
        }
    }
}
