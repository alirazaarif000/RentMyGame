using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RMG.BLL;
using RMG.Models;
using RMG.Models.ViewModels;
using System.Security.Claims;

namespace RMG.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class BuySubscriptionController : Controller
    {
        private readonly SubscriptionBll _subscriptionBll;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationUserBll _applicationUserBll;
        public BuySubscriptionController(SubscriptionBll subscriptionBll, SignInManager<IdentityUser> signInManager, ApplicationUserBll applicationUserBll)
        {
            _subscriptionBll = subscriptionBll;
            _signInManager = signInManager;
            _applicationUserBll = applicationUserBll;
        }

        public IActionResult Index()
        {
            SubscriptionVM subscriptionVM = new()
            {
                Subscription= _subscriptionBll.GetAllSubscription().Data,
                SubscribedId= null,
                
			};
            if (_signInManager.IsSignedIn(User))
            {
				var claimsIdentity = (ClaimsIdentity)User.Identity;
				var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                ApplicationUser user= _applicationUserBll.GetApplicationUser(userId).Data;
                if (user.SubscriptionId != null) {
                    subscriptionVM.SubscribedId = user.SubscriptionId;
                }
			}
            return View(subscriptionVM);
        }
        [Authorize]
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
