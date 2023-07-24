using APIProject.DTO.Comment;
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
    }
}
