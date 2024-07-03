using RMG.DAL.Repository.IRepository;
using RMG.Models;
using RMG.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.BLL
{
    public class DashboardBll
    {
        private readonly IUnitOfWork _uow;
        private readonly SubscriptionHistoryBll _SubhistoryBll;
        private readonly GameBll _GameBll;
        private readonly ApplicationUserBll _applicationUserBll;
        private readonly RentalBll _rentalBll;
        public DashboardBll(IUnitOfWork uow, SubscriptionHistoryBll SubhistoryBll, GameBll gameBll, ApplicationUserBll applicationUserBll)
        {
            _uow = uow;
            _SubhistoryBll = SubhistoryBll;
            _GameBll = gameBll;
            _applicationUserBll = applicationUserBll;
        }

        public Result<object> GetDashboardStats()
        {
            DateTime Days30Back = DateTime.Now.AddDays(-30);
            Result<List<SubscriptionHistory>> subscriptions = _SubhistoryBll.GetAllSubscriptionHistory();
            double TotalEarning = subscriptions.Data
            .Where(sh => sh.Subscription != null && sh.StartDate >= Days30Back)
            .Sum(sh => sh.Subscription.Price);

            List<Game> games = _GameBll.GetAllGame().Data;
            int GameCount = games.Where(g => g.CreatedDate >= Days30Back).Count();

            List<ApplicationUser> users = _applicationUserBll.GetAllApplicationUser().Data;
            int UserCount = users.Where(g => g.CreatedDate >= Days30Back).Count();

            int RentalCount = _uow.Rental.GetAll(r => r.RentalDate >= Days30Back).Count();

            return new Result<object>
            {
                Status = true,
                Data = new
                {
                    TotalEarning,
                    GameCount,
                    UserCount,
                    RentalCount
                },
                StatusCode = 200
            };
        }
        public Result<List<Game>> GetlatestProducts()
        {
            List<Game> games = _GameBll.GetAllGame().Data.OrderByDescending(g => g.CreatedDate).Take(5).ToList();
            return new Result<List<Game>>
            {
                Status = true,
                Data = games,
                StatusCode = 200
            };
        }
    }
}
