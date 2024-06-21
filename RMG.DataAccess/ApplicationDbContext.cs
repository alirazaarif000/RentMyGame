using Microsoft.EntityFrameworkCore;
using RMG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.DAL
{
	public class ApplicationDbContext:DbContext
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Game> Games { get; set; }
    }
}
