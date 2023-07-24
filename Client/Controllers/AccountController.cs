using APIProject.Controllers;
using APIProject.DTO.Category;
using APIProject.DTO.Comment;
using APIProject.DTO.Movie;
using APIProject.DTO.PurchasedMovie;
using APIProject.DTO.Transaction;
using APIProject.DTO.User;
using AutoMapper;
using BusinessObjects;
using Client.Models;
using Client.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Client.Controllers
{
    public class AccountController : Controller
    {
        public async Task<IActionResult> Index()
        {
            try
            {
                var profile = await HttpUtils.GetObject<GetUserResponseDTO>("/Profile");

                int userId = profile.UserId;
                var transaction = await HttpUtils.GetObject<ODataReponseModel<GetTransactionResponseDTO>>($"odata/Transactions?$filter=UserId eq {userId}&$OrderBy= TransactionDate desc");
                var transactionCount = await HttpUtils.GetObject<int>($"odata/PurchasedMovies/$count?$filter=UserId eq {userId}");
                ViewData["transaction"] = transaction;
                ViewData["transactionCount"] = ((int)Math.Ceiling(transactionCount * 1.0 / 5));             
                
                var purchaseMovie = await HttpUtils.GetObject<ODataReponseModel<GetPurchasedMovieResponseDTO>>($"odata/PurchasedMovies?$filter=UserId eq {userId}&expand=Movie&$OrderBy= PurchasedTime desc");
                var purchaseMovieCount = await HttpUtils.GetObject<int>($"odata/PurchasedMovies/$count?$filter=UserId eq {userId}");
                ViewData["purchaseMovie"] = purchaseMovie;
                ViewData["purchaseMovieCount"] = ((int)Math.Ceiling(purchaseMovieCount * 1.0 / 5));
                return View(profile);
            }
            catch
            {
                return RedirectPermanent("/Home/Index");
            }
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                GetUserResponseDTO responseDTO = await HttpUtils.PostAsync<GetUserResponseDTO>("ChangePassword", new StringContent(JsonSerializer.Serialize(new UserChangePasswordRequestDTO
                {
                    Password = model.Password,
                    NewPassword = model.NewPassword
                }), Encoding.UTF8, "application/json"));
                ModelState.AddModelError("ConfirmNewPassword", "Success");
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                string message = StringUtils.GetMessageFromErrorResponse(ex.Message);
                ModelState.AddModelError("ConfirmNewPassword", message);
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var profile = await HttpUtils.GetObject<GetUserResponseDTO>($"/Profile");
            ViewData["profile"] = profile;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Profile([FromForm] UpdateProfileModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                ModelState.AddModelError("LastName", "success");
                return View();
            } catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                //string message = StringUtils.GetMessageFromErrorResponse(ex.Message);
                //ModelState.AddModelError("ConfirmNewPassword", message);
            }
            return View(model);
        }
    }
}
