using Microsoft.AspNetCore.Mvc;
using RMG.Models;

namespace RMG.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GenreController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //Api's

        public Result<Genre> GetAll() { 
        
            var 
        }
    }
}
