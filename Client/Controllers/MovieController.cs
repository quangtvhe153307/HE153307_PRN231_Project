using APIProject.DTO.Category;
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
                //movie
                var movie = await HttpUtils.GetObject<GetMovieResponseDTO>($"odata/Movies/{id}?$expand=Categories,MovieSeasons($expand=MovieEpisodes)");

                //comment
                var comment = await HttpUtils.GetObject<ODataReponseModel<GetCommentResponseDTO>>("/odata/Comments?$expand=User&$orderby=CommentedDate desc");
                ViewData["comment"] = comment;

                //incase user is not permitted
                try
                {
                    //episodeUrl
                    var episodeUrl = await HttpUtils.GetObject<string>("/MovieSource/" + episodeId);
                    ViewData["url"] = episodeUrl;
                    ViewData["isPermitted"] = true;
                }
                catch (Exception ex)
                {
                    ViewData["isPermitted"] = false;
                }

                //related movie
                var movieCategories = movie.Categories as List<GetCategoryResponseDTO>;
                var relatedMovieQuery = "";
                relatedMovieQuery += $"c/CategoryId eq {movieCategories[0].CategoryId} ";
                for (int i = 1; i < movieCategories.Count; i++)
                {
                    relatedMovieQuery += $"or c/CategoryId eq {movieCategories[i].CategoryId}";
                }
                var relatedMovies = await HttpUtils.GetObject<ODataReponseModel<GetMovieResponseDTO>>("https://localhost:7038/odata/Movies?$filter=Categories/any(c: " + relatedMovieQuery + ")&$orderby=ViewCount desc&$top=10");
                ViewData["relatedMovies"] = relatedMovies.Value;

                return View(movie);
            } catch {
            }

            return RedirectPermanent("/Home/Index");
        }
    }
}
