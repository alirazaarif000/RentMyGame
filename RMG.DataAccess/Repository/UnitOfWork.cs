using RMG.DAL.Repository.IRepository;
using RMG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.DAL.Repository
{
    public class UnitOfWork :IUnitOfWork
    {
        private ApplicationDbContext _context;
        public IGenreRepository Genre { get; private set; }
        public IPlatformRepository Platform { get; private set; }
        public IGameRepository Game { get; private set; }
        public ISubscriptionRepository Subscription { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IRentalRepository Rental { get; private set; }
        public ISubscriptionHistoryRepository SubscriptionHistory { get; private set; }
        public IReviewRepository Review { get; private set; }
        public INotificationRepository Notification { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Genre= new GenreRepository(_context);
            Platform= new PlatformRepository(_context);
            Game= new GameRepository(_context);
            Subscription = new SubscriptionRepository(_context);
			ApplicationUser = new ApplicationUserRepository(_context);
			Rental = new RentalRepository(_context);
			SubscriptionHistory = new SubscriptionHistoryRepository(_context);
			Review = new ReviewRepository(_context);
            Notification = new NotificationRepository(_context);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
