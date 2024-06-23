using Microsoft.AspNetCore.Mvc;
using RMG.BLL;
using RMG.Models;
using System.Security.Claims;

namespace RMG.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class BuySubscriptionController : Controller
    {
        private readonly SubscriptionBll _subscriptionBll;
        public BuySubscriptionController(SubscriptionBll subscriptionBll)
        {
            _subscriptionBll = subscriptionBll;
        }

        public IActionResult Index()
        {
            List<Subscription> subscription= _subscriptionBll.GetAllSubscription().Data;
            return View(subscription);
        }

        public IActionResult Buy(int id)
        {
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            Result<object> result = _subscriptionBll.BuySubscription(id, userId);
            if (result.Status == true) {
            TempData["success"] = "Subscription Bought Successfully";
            return RedirectToAction("Index","Games");
            }
			TempData["error"] = "An error occured";
            return RedirectToAction("Index", "Games");
		}
    }
}
