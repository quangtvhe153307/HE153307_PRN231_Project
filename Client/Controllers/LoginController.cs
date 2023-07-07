using APIProject.DTO;
using Client.Models;
using Client.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Client.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient client = null;

        public LoginController(HttpClient httpClient)
        {
            client = httpClient;
            ViewData["Title"] = "Login";
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string strQuery = $"authenticate";

            HttpContent httpContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await client.PostAsync("authenticate", httpContent);
            string strData = await responseMessage.Content.ReadAsStringAsync();
            if (!responseMessage.IsSuccessStatusCode)
            {
                string message = StringUtils.GetMessageFromErrorResponse(strData);
                ModelState.AddModelError("Password", message);
                return View(model);
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            AuthenticateResponse response = JsonSerializer.Deserialize<AuthenticateResponse>(strData, options);

            JWTUtils.SetAccessToken(Response, response.AccessToken);
            JWTUtils.SetRefreshToken(responseMessage, Response);
            //return View(model);
            return RedirectPermanent("/Home/Index");
        }
    }
}
