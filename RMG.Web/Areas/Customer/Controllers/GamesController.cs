using Microsoft.AspNetCore.Mvc;
using RMG.BLL;
using RMG.Models;

namespace RMG.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class GamesController : Controller
    {
        private readonly GameBll _gameBll;
        public GamesController(GameBll gameBll)
        {
            _gameBll = gameBll;
        }
        public IActionResult Index()
        {
            Result<List<Game>> games= _gameBll.GetAllGame();
            return View(games.Data);
        }
        public IActionResult Detail(int id)
        {
            Game game = _gameBll.GetGame(id).Data;
            return View(game);
        }

    }
}
