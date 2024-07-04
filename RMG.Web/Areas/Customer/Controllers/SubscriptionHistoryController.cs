using Microsoft.AspNetCore.Mvc;
using RMG.BLL;
using RMG.DAL.Repository.IRepository;
using RMG.Models;
using RMG.Utility;
using System.Security.Claims;

namespace RMG.Web.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class SubscriptionHistoryController : Controller
	{
		public readonly IUnitOfWork _uow;

        public SubscriptionHistoryController( IUnitOfWork uow)
        {
            _uow = uow;
        }
        public IActionResult Index()
		{
			return View();
		}


		//Api

		public IActionResult GetSubscriptionHistory()
		{
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<SubscriptionHistory> subscriptionHistory = _uow.SubscriptionHistory.GetAll(s => s.ApplicationUserId == userId, IncludeProperties: "Subscription").ToList();

			return Json(subscriptionHistory);
        }
	}
}
