using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMG.BLL;
using RMG.Models;
using RMG.Utility;

namespace RMG.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ReviewController : Controller
    {
        private readonly ReviewBll _reviewBll;
        public ReviewController(ReviewBll reviewBll)
        {
            _reviewBll = reviewBll;
        }
        public IActionResult Index()
        {
            return View();
        }

        //Api's
        public IActionResult getall()
        {
            var reviews = _reviewBll.ReviewsForApproval().Data;
            return Json(reviews);
        }
        public IActionResult ApproveReview(int id) 
        {
            Result<object> result= _reviewBll.ApproveReview(id);
            if (result.Status)
            {
                TempData["success"] = result.Message;
            }
            return RedirectToAction("Index");
          
        }
        public IActionResult RejectReview(int id)
        {
            Result<object> result = _reviewBll.RejectReview(id);
            if (result.Status)
            {
                TempData["success"] = result.Message;
            }
            return RedirectToAction("Index");
        }

    }
}