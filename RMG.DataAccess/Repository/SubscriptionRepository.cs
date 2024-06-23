using RMG.DAL.Repository.IRepository;
using RMG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.DAL.Repository
{
    public class SubscriptionRepository:Repository<Subscription>,  ISubscriptionRepository
    {
        private readonly ApplicationDbContext _context;
        public SubscriptionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Subscription subscription)
        {
            _context.Subscriptions.Update(subscription);
        }
    }
}
