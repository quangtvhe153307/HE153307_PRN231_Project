using APIProject.DTO.Category;
using APIProject.DTO.Comment;
using APIProject.DTO.PurchasedMovie;
using APIProject.DTO.Transaction;
using APIProject.DTO.User;
using BusinessObjects;
using Client.Models;
using Client.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("DashBoard", "Admin");
        }
        public IActionResult DashBoard()
        {
            return View();
        }
        public async Task<IActionResult> Comments()
        {
            try
            {
                var comment = await HttpUtils.GetObject<ODataReponseModel<GetCommentResponseDTO>>($"/odata/Comments?$expand=Movie,User");
                var commentCount = 0;
                commentCount = await HttpUtils.GetObject<int>($"odata/Comments/$count");
                ViewData["commentCount"] = ((int)Math.Ceiling(commentCount * 1.0 / 10));
                return View(comment);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Login");
            }
        }        
        public async Task<IActionResult> Transactions()
        {
            try
            {
                var comment = await HttpUtils.GetObject<ODataReponseModel<GetTransactionResponseDTO>>($"/odata/Transactions?$expand=User&$Orderby= TransactionId desc");
                var commentCount = 0;
                commentCount = await HttpUtils.GetObject<int>($"odata/Transactions/$count");
                ViewData["transactionCount"] = ((int)Math.Ceiling(commentCount * 1.0 / 10));
                return View(comment);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Login");
            }
        }        
        public async Task<IActionResult> Purchased()
        {
            try
            {
                var comment = await HttpUtils.GetObject<ODataReponseModel<GetPurchasedMovieResponseDTO>>($"/odata/PurchasedMovies?$expand=User,Movie&$Orderby= TransactionId desc");
                var commentCount = 0;
                commentCount = await HttpUtils.GetObject<int>($"odata/Transactions/$count");
                ViewData["transactionCount"] = ((int)Math.Ceiling(commentCount * 1.0 / 10));
                return View(comment);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Login");
            }
        }        
        public async Task<IActionResult> Categories()
        {
            try
            {
                var comment = await HttpUtils.GetObject<ODataReponseModel<GetCategoryResponseDTO>>($"/odata/Categories?&$Orderby= CategoryId asc");
                var commentCount = 0;
                commentCount = await HttpUtils.GetObject<int>($"odata/Categories/$count");
                ViewData["categoriesCount"] = ((int)Math.Ceiling(commentCount * 1.0 / 10));
                return View(comment);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Login");
            }
        }        
        public async Task<IActionResult> Users()
        {
            try
            {
                var comment = await HttpUtils.GetObject<ODataReponseModel<GetUserResponseDTO>>($"/odata/Users?$expand= Role &$Orderby= UserId desc");
                var commentCount = 0;
                commentCount = await HttpUtils.GetObject<int>($"odata/Users/$count");
                ViewData["userCount"] = ((int)Math.Ceiling(commentCount * 1.0 / 10));
                return View(comment);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}
