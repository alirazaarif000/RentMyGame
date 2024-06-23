using RMG.DAL.Repository.IRepository;
using RMG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.DAL.Repository
{
    public class RentalRepository:Repository<Rental>,  IRentalRepository
    {
        private readonly ApplicationDbContext _context;
        public RentalRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Rental rental)
        {
            _context.Rentals.Update(rental);
        }
    }
}
