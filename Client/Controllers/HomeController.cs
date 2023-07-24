using APIProject.DTO.Movie;
using Client.Models;
using Client.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient client = null;
        public HomeController(ILogger<HomeController> logger, HttpClient client)
        {
            _logger = logger;
            this.client = client;
        }

        public async Task<IActionResult> Index()
        {
            string strQuery = $"/MovieRanking/2";
            try
            {
                var list = await HttpUtils.GetList<GetMovieByRankResponseDTO>(strQuery);
                if (list != null)
                {
                    ViewBag.RankedMovie = list;
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            

            List<GetMovieResponseDTO> newestMovie = null;
            strQuery = "api/Movies/List?$OrderBy= UpdatedDate desc&$top=12";
            try
            {
                newestMovie = await HttpUtils.GetList<GetMovieResponseDTO>(strQuery);
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(newestMovie);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}