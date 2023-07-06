
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
            //CreateUserRequestDTO createUserRequestDTO = new CreateUserRequestDTO
            //{
            //    EmailAddress = model.EmailAddress,
            //    Password = model.Password,
            //    Source = "",
            //    FirstName = "",
            //    MiddleName = "",
            //    LastName = "",
            //    RoleId = 1,
            //    PublisherId = 1,
            //    HireDate = DateTime.Today
            //};
            //string url = "https://localhost:7111/odata/Users";

            //HttpContent content = new StringContent(JsonSerializer.Serialize(createUserRequestDTO), Encoding.UTF8, "application/json");
            //HttpResponseMessage response = await client.PostAsync(url, content);
            //if(response.IsSuccessStatusCode)
            //{
            //    string strData = await response.Content.ReadAsStringAsync();
            //    GetUserResponseDTO getUserResponseDTO = JsonSerializer.Deserialize<GetUserResponseDTO>(strData);
            //    HttpContext.Session.Remove("UserId");

            //    HttpContext.Session.SetInt32("UserId", getUserResponseDTO.UserId);

            //    return RedirectPermanent("/Book");
            //}

            //ModelState.AddModelError("EmailAddress", "Error");
            return View(model);
        }
    }
}
