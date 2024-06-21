using RMG.DAL.Repository.IRepository;
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
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Genre= new GenreRepository(_context);
            Platform= new PlatformRepository(_context);
            Game= new GameRepository(_context);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
