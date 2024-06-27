using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using RMG.BLL;
using RMG.Models;
using RMG.Models.ViewModels;
using RMG.Utility;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RMG.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class GameController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly GameBll _gameBll;
        public GameController(GameBll gameBll, IWebHostEnvironment webHostEnvironment)
        {
            _gameBll = gameBll;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            GameVM game = _gameBll.BindGameDropdowns().Data;
            if (id == null || id == 0)
            {
                //create
                return View(game);
            }
            else
            {
                //update
                game.Game = _gameBll.GetGame(id).Data;
                return View(game);
            }
        }
        [HttpPost]  
        public IActionResult Upsert(GameVM gameVM, IFormFile? file) 
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string rootPath = _webHostEnvironment.WebRootPath;
                    string filePath = Path.Combine(rootPath, @"admin\images\games");

                    using (var fileStream = new FileStream(Path.Combine(filePath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    if (!System.String.IsNullOrEmpty(gameVM.Game.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(rootPath, gameVM.Game.ImageUrl.Trim('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    gameVM.Game.ImageUrl = @"\admin\images\games\" + fileName;
                }
                if (gameVM.Game.Id == 0)
                {
                    _gameBll.AddGame(gameVM.Game);
                    TempData["success"] = "Game Successfully Created";
                }
                else
                {
                    _gameBll.UpdateGame(gameVM.Game);
                    TempData["success"] = "Game Successfully Updated";
                }
                
                return RedirectToAction("Index");
            }
            else
            {
                return View(gameVM);
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
