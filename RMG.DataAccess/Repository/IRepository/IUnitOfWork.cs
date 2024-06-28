using RMG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.DAL.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IGenreRepository Genre { get; }
        IPlatformRepository Platform { get; }
        IGameRepository Game { get; }
        ISubscriptionRepository Subscription { get; }
		IApplicationUserRepository ApplicationUser { get; }
		IRentalRepository Rental { get; }
		ISubscriptionHistoryRepository SubscriptionHistory { get; }
		IReviewRepository Review { get; }
		INotificationRepository Notification { get; }
       
        void Save();
    }
}
