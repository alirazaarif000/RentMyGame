using RMG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.DAL.Repository.IRepository
{
    public interface ISubscriptionHistoryRepository:IRepository<SubscriptionHistory>
    {
        void Update(SubscriptionHistory subscriptionHistory);
    }
}
