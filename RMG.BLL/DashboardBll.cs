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

        public Result<object> Get30DaysEarning()
        {
            Result<List<SubscriptionHistory>> subscriptions = _SubhistoryBll.GetAllSubscriptionHistory();
            double TotalEarning = subscriptions.Data
            .Where(sh => sh.Subscription != null && sh.StartDate >= DateTime.Now.AddDays(-30))
            .Sum(sh => sh.Subscription.Price);
            return new Result<object>
            {

                Status = true,
                Data = TotalEarning,
                StatusCode = 200
            };
        }

        public Result<object> GetNewReleased()
        {
            List<Game> games = _GameBll.GetAllGame().Data;
            int gameCount = games.Where(g => g.CreatedDate >= DateTime.Now.AddDays(-30)).Count();
            return new Result<object>
            {

                Status = true,
                Data = gameCount,
                StatusCode = 200
            };
        }

        public Result<object> NewlyRegisteredUser()
        {
            List<ApplicationUser> users = _applicationUserBll.GetAllApplicationUser().Data;
            int UserCount = users.Where(g => g.CreatedDate >= DateTime.Now.AddDays(-30)).Count();
            return new Result<object>
            {

                Status = true,
                Data = UserCount,
                StatusCode = 200
            };
        }

        public Result<object> RentedGames()
        {
            int RentalCount= _uow.Rental.GetAll(r=>r.RentalDate >= DateTime.Now.AddDays(-30)).Count();
            return new Result<object>
            {

                Status = true,
                Data = RentalCount,
                StatusCode = 200
            };
        }
    }
}
