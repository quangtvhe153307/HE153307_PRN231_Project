using APIProject.DTO.Category;
using APIProject.DTO.Comment;
using APIProject.DTO.Movie;
using APIProject.DTO.PurchasedMovie;
using APIProject.DTO.Transaction;
using APIProject.DTO.User;
using Client.Models;
using Client.Utils;
using Microsoft.AspNetCore.Mvc;

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
    }
}
