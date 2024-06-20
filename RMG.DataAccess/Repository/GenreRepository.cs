using RMG.DAL.Repository.IRepository;
using RMG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.DAL.Repository
{
    public class GenreRepository:Repository<Genre>,  IGenreRepository
    {
        private readonly ApplicationDbContext _context;
        public GenreRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Genre genre)
        {
            _context.Genres.Update(genre);
        }
    }
}
