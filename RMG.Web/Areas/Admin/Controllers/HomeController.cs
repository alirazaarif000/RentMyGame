using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMG.BLL;
using RMG.Models;
using RMG.Utility;
using System.Diagnostics;

namespace RMG.Web.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class HomeController : Controller
	{
		private readonly DashboardBll _dashboardBll;

		public HomeController(DashboardBll dashboardBll)
		{
			_dashboardBll = dashboardBll;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult GetStats()
		{
			Object stats = _dashboardBll.GetDashboardStats().Data;
			return Json(stats);
		}
		public IActionResult GetLatestProduct()
		{
			List<Game> games = _dashboardBll.GetlatestProducts().Data;
			return Json(games);
		}
	}
}
