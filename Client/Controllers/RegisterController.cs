
using APIProject.DTO.User;
using Client.Models;
using Client.Utils;
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
            string strData = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                JsonSerializerOptions option = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                EmailConfirmModel emailConfirm = JsonSerializer.Deserialize<EmailConfirmModel>(strData, option);
                return RedirectToAction("EmailConfirmation");
            }
            string message = StringUtils.GetMessageFromErrorResponse(strData);
            ModelState.AddModelError("EmailAddress", message);
            return View(model);
        }
        [HttpGet("Register/ConfirmEmail/{UserId}/{Token}")]
        public async Task<IActionResult> ConfirmEmailAsync(EmailConfirmModel emailConfirm)
        {
            bool completed = false;
            if (!ModelState.IsValid)
            {
                ViewData["completed"] = completed;
                return View(emailConfirm);
            }
            string url = $"ConfirmEmail/{emailConfirm.UserId}/{emailConfirm.Token}";

            HttpResponseMessage response = await client.GetAsync(url);
            string strData = await response.Content.ReadAsStringAsync();
            string message = StringUtils.GetMessageFromErrorResponse(strData);
            ViewData["message"] = message;
            if (response.IsSuccessStatusCode)
            {
                completed= true;
            }
            ViewData["completed"] = completed;
            return View();
        }
        [HttpGet]
        public IActionResult EmailConfirmation()
        {
            return View();
        }
    }
}
