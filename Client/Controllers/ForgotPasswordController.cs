using APIProject.DTO;
using Client.Models;
using Client.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

namespace Client.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private readonly HttpClient client = null;

        public ForgotPasswordController(HttpClient client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string strQuery = $"ResetPassword";

            HttpContent httpContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await client.PostAsync(strQuery, httpContent);
            string strData = await responseMessage.Content.ReadAsStringAsync();

            if (responseMessage.IsSuccessStatusCode)
            {
                ViewData["success"] = true;
            }
            string message = StringUtils.GetMessageFromErrorResponse(strData);
            ModelState.AddModelError("Email", message);
            return View(model);
        }
    }
}
