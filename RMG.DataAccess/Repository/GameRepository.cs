using RMG.DAL.Repository.IRepository;
using RMG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.DAL.Repository
{
    public class GameRepository:Repository<Game>,  IGameRepository
    {
        private readonly ApplicationDbContext _context;
        public GameRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Game game)
        {
            _context.Games.Update(game);
        }
    }
}
