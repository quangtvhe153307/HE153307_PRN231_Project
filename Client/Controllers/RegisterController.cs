
using APIProject.DTO.User;
using Client.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Client.Controllers
{
    public class RegisterController : Controller
    {
        private readonly HttpClient client = null;

        public RegisterController(HttpClient httpClient)
        {
            client = httpClient;
            //UserApiUrl = "https://localhost:7111/odata/Users";
            ViewData["Title"] = "Register";
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] RegisterViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            CreateUserRequestDTO createUserRequestDTO = new CreateUserRequestDTO
            {
                Email = model.EmailAddress,
                Password = model.Password,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            string url = "RegisterUser";

            HttpContent content = new StringContent(JsonSerializer.Serialize(createUserRequestDTO), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions option = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                EmailConfirmModel emailConfirm = JsonSerializer.Deserialize<EmailConfirmModel>(strData, option);
                return RedirectToAction("EmailConfirmation");
            }

            ModelState.AddModelError("EmailAddress", "Error");
            return View(model);
        }
        [HttpGet("Register/ConfirmEmail/{UserId}/{Token}")]
        public async Task<IActionResult> ConfirmEmailAsync(EmailConfirmModel emailConfirm)
        {
            if (!ModelState.IsValid)
            {
                return View(emailConfirm);
            }
            string url = $"ConfirmEmail/{emailConfirm.UserId}/{emailConfirm.Token}";

            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                //return RedirectToAction("Index", "Home");
                ViewData["message"] = "success";
            } else
            {
                ViewData["message"] = "error";
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> EmailConfirmation()
        {
            return View();
        }
    }
}
