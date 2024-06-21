using Microsoft.AspNetCore.Mvc;
using RMG.BLL;
using RMG.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RMG.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GameController : Controller
    {
        private readonly GameBll _gameBll;
        public GameController(GameBll gameBll) 
        {
            _gameBll = gameBll;    
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
                return View(new Game());
            }
            else
            {
                //update
                Result<Game> game = _gameBll.GetGame(id);
                return View(game.Data);
            }
        }
        [HttpPost]  
        public IActionResult Upsert(Game game) 
        {
            if (ModelState.IsValid)
            {
                if (game.Id == 0)
                {
                    _gameBll.AddGame(game);
                }
                else
                {
                    _gameBll.UpdateGame(game);
                }
                TempData["success"] = "Game Successfully Created";
                return RedirectToAction("Index");
            }
            else
            {
                return View(game);
            }
        }

        //Api's

        [HttpGet]
        public IActionResult GetAll()
        {
            var Games = _gameBll.GetAllGame();
            return Json(Games);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result= _gameBll.DeleteGame(id);
            return Json(result);
        }

    }
}
