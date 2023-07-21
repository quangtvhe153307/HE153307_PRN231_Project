using APIProject.DTO;
using APIProject.DTO.Movie;
using APIProject.ZaloPay.Models;
using APIProject.ZaloPay;
using Client.Models;
using Client.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Client.Controllers
{
    public class BalanceController : Controller
    {
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddBalanceModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var response = await HttpUtils.PostAsync<AddBalanceResponseModel>("api/Balances", new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));
                HttpContext.Session.SetString("orderurl", response.OrderUrl);
                HttpContext.Session.SetString("QRCodeBase64Image", QRCodeHelper.CreateQRCodeBase64Image(response.OrderUrl));
                HttpContext.Session.SetString("apptransid", response.Apptransid);
                return View();
            } catch (Exception ex)
            {

            }
            return View();
        }
    }
}
