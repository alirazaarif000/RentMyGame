using RMG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.DAL.Repository.IRepository
{
    public interface IRentalRepository:IRepository<Rental>
    {
        void Update(Rental rental);
    }
}
