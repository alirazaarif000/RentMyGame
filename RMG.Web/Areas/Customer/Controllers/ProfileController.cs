using Microsoft.AspNetCore.Mvc;

namespace RMG.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
