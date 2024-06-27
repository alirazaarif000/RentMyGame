using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RMG.BLL;
using RMG.Models;
using RMG.Models.ViewModels;
using RMG.Utility;
using System.Net.NetworkInformation;
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
        private readonly SignInManager<IdentityUser> _signInManager;
        public GamesController(GameBll gameBll, ApplicationUserBll userBll, RentalBll rentalBll, ReviewBll reviewBll, GenreBll genreBll, PlatformBll platformBll, SignInManager<IdentityUser> signInManager)
        {
            _gameBll = gameBll;
            _userBll = userBll;
            _rentalBll = rentalBll;
            _reviewBll = reviewBll;
            _genreBll = genreBll;
            _platformBll = platformBll;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Detail(int id)
        {
            Rental rental = new Rental();
            if (_signInManager.IsSignedIn(User)) 
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                rental = _rentalBll.GetRentalByUser(userId, id, SD.ActiveStatus).Data;
            }
          
            RentGameVM rentGameVM = new()
            {
                Game = _gameBll.GetGame(id).Data,
                Rental = rental,
            };

            return View(rentGameVM);
        }
        [Authorize]
        public IActionResult RentGame(int gameId) 
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            Result<object> result= _gameBll.RentGame(userId, gameId);
            if (result.Status)
            {
                TempData["success"] = result.Message;
            }
            else
            {
                TempData["error"] = result.Message;
            }
            return RedirectToAction("Detail", "Games", new { id=gameId });
        }
        [Authorize]
        public IActionResult RemoveRent(int id, int gameId)
        {
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
			Result<object> result = _gameBll.ReturnGame(userId, gameId);
            if (result.Status)
            {
                TempData["success"] = result.Message;
            }
            else
            {
                TempData["error"] = result.Message;
            }
            return RedirectToAction("Detail", "Games", new { id=gameId });
        }
        [Authorize]
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
            rentGameVM.Review.Status = SD.ApprovalPendingReview;
            rentGameVM.Review.GameId = rentGameVM.Game.Id;
            rentGameVM.Review.ReviewDate= DateOnly.FromDateTime(DateTime.Now);
            Result<object> result = _reviewBll.AddReview(rentGameVM.Review);
            if (result.Status)
            {
                TempData["success"] = result.Message;
            }
            else
            {
                TempData["error"] = result.Message;
            }
            return RedirectToAction("Detail", "Games", new { id=rentGameVM.Game.Id });
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

        public IActionResult GetFilteredGames(string genres, string platforms, int? rating, DateOnly? fromDate, DateOnly? toDate)
        {
            var games = _gameBll.GetAllGame().Data;

            if (!string.IsNullOrEmpty(genres))
            {
                var genreIds = genres.Split(',').Select(int.Parse).ToList();
                games = games.Where(g => genreIds.Contains(g.GenreId)).ToList();
            }

            if (!string.IsNullOrEmpty(platforms))
            {
                var platformIds = platforms.Split(',').Select(int.Parse).ToList();
                games = games.Where(g => platformIds.Contains(g.PlatformId)).ToList();
            }

            if (rating != null)
            {
                games = games.Where(g => g.Ratings >= rating).ToList();
            }
            if (toDate != null)
            {
                if (fromDate != null)
                {
                    games = games.Where(g => g.ReleaseDate >= fromDate && g.ReleaseDate <= toDate).ToList();
                }
                else
                {
                    games = games.Where(g => g.ReleaseDate <= toDate).ToList();
                }
            }

            return Json(games);
        }

    }
}
