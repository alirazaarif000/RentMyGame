using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMG.BLL;
using RMG.Models;
using RMG.Utility;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RMG.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class GenreController : Controller
    {
        private readonly GenreBll _genreBll;
        public GenreController(GenreBll genreBll) 
        {
            _genreBll = genreBll;    
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
                return View(new Genre());
            }
            else
            {
                //update
                Result<Genre> genre = _genreBll.GetGenre(id);
                return View(genre.Data);
            }
        }
        [HttpPost]  
        public IActionResult Upsert(Genre genre) 
        {
            if (ModelState.IsValid)
            {
                if (genre.Id == 0)
                {
                    _genreBll.AddGenre(genre);
                }
                else
                {
                    _genreBll.UpdateGenre(genre);
                }
                TempData["success"] = "Genre Successfully Created";
                return RedirectToAction("Index");
            }
            else
            {
                return View(genre);
            }
        }

        //Api's

        [HttpGet]
        public IActionResult GetAll()
        {
            var Genres = _genreBll.GetAllGenre();
            return Json(Genres);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result= _genreBll.DeleteGenre(id);
            return Json(result);
        }

    }
}
