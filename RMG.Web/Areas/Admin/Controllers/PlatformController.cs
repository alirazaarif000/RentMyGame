using Microsoft.AspNetCore.Mvc;
using RMG.BLL;
using RMG.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RMG.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PlatformController : Controller
    {
        private readonly PlatformBll _platformBll;
        public PlatformController(PlatformBll platformBll) 
        {
            _platformBll = platformBll;    
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                //create
                return View(new Platform());
            }
            else
            {
                //update
                Result<Platform> platform = _platformBll.GetPlatform(id);
                return View(platform.Data);
            }
        }
        [HttpPost]  
        public IActionResult Upsert(Platform platform) 
        {
            if (ModelState.IsValid)
            {
                if (platform.Id == 0)
                {
                    _platformBll.AddPlatform(platform);
                }
                else
                {
                    _platformBll.UpdatePlatform(platform);
                }
                TempData["success"] = "Platform Successfully Created";
                return RedirectToAction("Index");
            }
            else
            {
                return View(platform);
            }
        }

        //Api's

        [HttpGet]
        public IActionResult GetAll()
        {
            var Platforms = _platformBll.GetAllPlatform();
            return Json(Platforms);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result= _platformBll.DeletePlatform(id);
            return Json(result);
        }

    }
}
