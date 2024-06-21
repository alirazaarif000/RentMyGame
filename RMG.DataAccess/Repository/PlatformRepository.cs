using RMG.DAL.Repository.IRepository;
using RMG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.DAL.Repository
{
    public class PlatformRepository:Repository<Platform>,  IPlatformRepository
    {
        private readonly ApplicationDbContext _context;
        public PlatformRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Platform platform)
        {
            _context.Platforms.Update(platform);
        }
    }
}
