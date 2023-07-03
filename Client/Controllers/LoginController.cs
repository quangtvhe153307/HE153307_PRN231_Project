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

            if(!responseMessage.IsSuccessStatusCode)
            {
                ModelState.AddModelError("Password", "Invalid email address or password.");
                return View(model);
            }

            string strData = await responseMessage.Content.ReadAsStringAsync();

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            AuthenticateResponse response = JsonSerializer.Deserialize<AuthenticateResponse>(strData, options);

            JWTUtils.SetAccessToken(Response, response.AccessToken);
            JWTUtils.SetRefreshToken(responseMessage, Response);
            //dynamic temp = JObject.Parse(strData);
            //JArray lst = temp.value;
            //if (!lst.HasValues)
            //{
            //    ModelState.AddModelError("Password", "Invalid email address or password.");
            //    return View(model);
            //}
            //GetUserResponseDTO user = ((JArray)temp.value)
            //    .Select(x => new GetUserResponseDTO
            //    {
            //        UserId = (int)x["UserId"],
            //        EmailAddress = (string)x["EmailAddress"]
            //    })
            //    .First();
            //HttpContext.Session.Remove("UserId");

            //HttpContext.Session.SetInt32("UserId", user.UserId);

            return View(model);
        }
    }
}
