using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        private readonly SubscriptionHistoryBll _subscriptionHistoryBll;
        public BuySubscriptionController(SubscriptionBll subscriptionBll, SignInManager<IdentityUser> signInManager, ApplicationUserBll applicationUserBll, SubscriptionHistoryBll subscriptionHistoryBll)
        {
            _subscriptionBll = subscriptionBll;
            _signInManager = signInManager;
            _applicationUserBll = applicationUserBll;
            _subscriptionHistoryBll = subscriptionHistoryBll;
        }

        public IActionResult Index()
        {
   //         SubscriptionVM subscriptionVM = new()
   //         {
   //             Subscription= _subscriptionBll.GetAllSubscription().Data,
   //             SubscriptionHistory = new SubscriptionHistory(),
                
			//};
   //         if (_signInManager.IsSignedIn(User))
   //         {
			//	var claimsIdentity = (ClaimsIdentity)User.Identity;
			//	var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
   //             ApplicationUser user= _applicationUserBll.GetApplicationUser(userId).Data;
   //             subscriptionVM.SubscriptionHistory = _subscriptionHistoryBll.GetUserSubscription(userId).Data;
			//}
            return View();
        }
        [Authorize]
        public IActionResult Buy(SubscribeDTO sub)
        {
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            Result<object> result = _subscriptionBll.BuySubscription(sub, userId);
            return Json(result.Message);
		}
        [Authorize]
        public IActionResult GetUserSubscription()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            SubscriptionHistory subscription = _subscriptionHistoryBll.GetUserSubscription(userId).Data;
            return Json(subscription);
        }


        //Api's

        public IActionResult GetSubscriptionsData()
        {
            List<Subscription> subscriptions = _subscriptionBll.GetAllSubscription().Data;
            return Json(subscriptions);
        }
    }
}
