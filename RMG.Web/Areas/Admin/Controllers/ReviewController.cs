using Microsoft.AspNetCore.Mvc;
using RMG.BLL;
using RMG.Models;

namespace RMG.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
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
        [HttpGet]
        public IActionResult getall()
        {
            List<Review> reviews = _reviewBll.ReviewsForApproval().Data;
            return Json(reviews);
        }
    }
}
