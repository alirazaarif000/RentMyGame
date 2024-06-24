using RMG.DAL.Repository.IRepository;
using RMG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.DAL.Repository
{
    public class SubscriptionHistoryRepository:Repository<SubscriptionHistory>,  ISubscriptionHistoryRepository
    {
        private readonly ApplicationDbContext _context;
        public SubscriptionHistoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(SubscriptionHistory subscriptionHistory)
        {
            _context.SubscriptionsHistory.Update(subscriptionHistory);
        }
    }
}
