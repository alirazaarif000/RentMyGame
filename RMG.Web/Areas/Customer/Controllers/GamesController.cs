using Microsoft.AspNetCore.Mvc;
using RMG.BLL;
using RMG.Models;
using RMG.Models.ViewModels;
using RMG.Utility;
using System.Security.Claims;

namespace RMG.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class GamesController : Controller
    {
        private readonly GameBll _gameBll;
        private readonly ApplicationUserBll _userBll;
        private readonly RentalBll _rentalBll;
        public GamesController(GameBll gameBll, ApplicationUserBll userBll, RentalBll rentalBll)
        {
            _gameBll = gameBll;
            _userBll = userBll;
            _rentalBll = rentalBll;
        }
        public IActionResult Index()
        {
            Result<List<Game>> games= _gameBll.GetAllGame();
            return View(games.Data);
        }
        public IActionResult Detail(int gameId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            RentGameVM rentGameVM = new()
            {
                Game = _gameBll.GetGame(gameId).Data,
                Rental = _rentalBll.GetRentalByUser(userId, gameId, SD.ActiveStatus).Data,
            };

            return View(rentGameVM);
        }

        public IActionResult RentGame(int gameId) 
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            Result<object> result= _gameBll.RentGame(userId, gameId);
            TempData["success"] = result.Message;
            return RedirectToAction("Detail", "Games", new { gameId });
        }

        public IActionResult RemoveRent(int id, int gameId)
        {
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
			Result<object> result = _gameBll.ReturnGame(userId, gameId);
            TempData["success"] = result.Message;
            return RedirectToAction("Detail", "Games", new { gameId });
        }

    }
}
