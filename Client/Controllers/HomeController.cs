using APIProject.DTO.Movie;
using Client.Models;
using Microsoft.AspNetCore.Mvc;
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
            var header = client.DefaultRequestHeaders.Authorization;
            if (header == null || header.ToString().Equals(""))
            {
                string accessToken = Request.Cookies["accessToken"];
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            }

            HttpResponseMessage responseMessage = await client.GetAsync(strQuery);
            if(responseMessage.IsSuccessStatusCode) {
                string strData = await responseMessage.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };
                List<GetMovieByRankResponseDTO> getMovieResponseDTOs = JsonSerializer.Deserialize<List<GetMovieByRankResponseDTO>>(strData, options);
                ViewBag.RankedMovie = getMovieResponseDTOs;
            }

            return View();
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