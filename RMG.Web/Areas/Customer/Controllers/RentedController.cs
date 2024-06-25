using Microsoft.AspNetCore.Mvc;
using RMG.BLL;
using RMG.Models;
using RMG.Utility;
using System.Security.Claims;

namespace RMG.Web.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class RentedController : Controller
	{
		private readonly RentalBll _rentalBll;
        public RentedController(RentalBll rentalBll)
        {
            _rentalBll = rentalBll;
        }
        public IActionResult Index()
		{
			return View();
		}

		//Api's

		public IActionResult getall()
		{
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            Result<List<Rental>> rentals = _rentalBll.GetAllRental(userId, SD.ActiveStatus);
            if (rentals.Data != null)
            {
                return Json(rentals.Data);
            }
            return Json(new Rental() { });
        }
	}
}
