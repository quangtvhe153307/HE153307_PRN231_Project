using APIProject.DTO.Category;
using APIProject.DTO.Comment;
using APIProject.DTO.Movie;
using BusinessObjects;
using Client.Models;
using Client.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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

                if (episodeId == 0 || episodeId == null)
                {
                    episodeId = movie.MovieSeasons.ToList()[0].MovieEpisodes.ToList()[0].EpisodeId;
                }
                //comment
                var comment = await HttpUtils.GetObject<ODataReponseModel<GetCommentResponseDTO>>($"/odata/Comments/{id}?$expand=User&$orderby=CommentedDate desc");
                ViewData["comment"] = comment;
                ViewData["episodeId"] = episodeId;

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
        [HttpGet]
        public async Task<IActionResult> Search(MovieSearchModel model)
        {
            if(model.Title is null)
            {
                model.Title = "";
            }
            ViewData["searchKey"] = model.Title;
            var categories = await HttpUtils.GetObject<ODataReponseModel<GetCategoryResponseDTO>>($"odata/Categories");
            if(model.Categories != null)
            {
                foreach (var item in categories.Value)
                {
                    if (model.Categories.Contains(item.CategoryId))
                    {
                        item.Selected = true;
                    }
                }
            }
            ViewData["categories"] = categories;
            //ViewData["selectedCategories"] = model.Categories != null ? model.Categories : new List<int>();
            string str = "";
            if(model.Categories != null)
            {
                var relatedMovieQuery = "";
                if(model.Categories != null && model.Categories.Count > 0)
                {
                    relatedMovieQuery += $"c/CategoryId eq {model.Categories[0]} ";
                }
                for (int i = 1; i < model.Categories.Count; i++)
                {
                    relatedMovieQuery += $"or c/CategoryId eq {model.Categories[i]}";
                }
                str = " and Categories/any(c: " + relatedMovieQuery + ")";
            }

            var movies = await HttpUtils.GetObject<ODataReponseModel<GetMovieResponseDTO>>($"odata/Movies?$filter=IsSingleEpisode eq false and contains(tolower(Title), tolower('{model.Title}'))"+ str + "&$orderby=UpdatedDate desc&$top=8");
            var movieCount = await HttpUtils.GetObject<int>($"odata/Movies/$count?$filter=IsSingleEpisode eq false and contains(tolower(Title), tolower('{model.Title}'))"+ str);
            ViewData["movieCount"] = ((int)Math.Ceiling(movieCount * 1.0 / 8));
            return View(movies);
        }
    }
}
