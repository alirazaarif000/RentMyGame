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
        private readonly ReviewBll _reviewBll;
        private readonly GenreBll _genreBll;
        private readonly PlatformBll _platformBll;
        public GamesController(GameBll gameBll, ApplicationUserBll userBll, RentalBll rentalBll, ReviewBll reviewBll, GenreBll genreBll, PlatformBll platformBll)
        {
            _gameBll = gameBll;
            _userBll = userBll;
            _rentalBll = rentalBll;
            _reviewBll = reviewBll;
            _genreBll = genreBll;
            _platformBll = platformBll;
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

        [HttpPost]
        [ActionName("Detail")]
        public IActionResult AddReview(RentGameVM rentGameVM)
        {
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (rentGameVM.Review.Rating == 0)
            {
                rentGameVM.Review.Rating = 5;
            }
            rentGameVM.Review.ApplicationUserId = userId;
            rentGameVM.Review.IsApproved = false;
            rentGameVM.Review.GameId = rentGameVM.Game.Id;
            rentGameVM.Review.ReviewDate= DateOnly.FromDateTime(DateTime.Now);
            Result<object> result = _reviewBll.AddReview(rentGameVM.Review);
            TempData["success"] = result.Message;
            return RedirectToAction("Detail", "Games", new { gameId=rentGameVM.Game.Id });
        }

        //Api's

        public IActionResult getall()
        {
            List<Game> games = _gameBll.GetAllGame().Data;
            return Json(games);
        }
        public IActionResult getgenres()
        {
            List<Genre> genres = _genreBll.GetAllGenre().Data;
            return Json(genres);
        }
        public IActionResult getplatforms()
        {
            List<Platform> platforms = _platformBll.GetAllPlatform().Data;
            return Json(platforms);
        }
    }
}
