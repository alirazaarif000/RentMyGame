using RMG.DAL.Repository.IRepository;
using RMG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.DAL.Repository
{
    public class ReviewRepository:Repository<Review>,  IReviewRepository
    {
        private readonly ApplicationDbContext _context;
        public ReviewRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Review review)
        {
            _context.Reviews.Update(review);
        }
    }
}
