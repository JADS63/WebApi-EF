using Entities;
using Microsoft.EntityFrameworkCore;

namespace ContextLib
{
    public class WtaContext : DbContext
    {
        public DbSet<PlayerEntity> Players { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=players.db");
        }
    }
}
