// ContextLib/WtaContext.cs
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ContextLib
{
    public class WtaContext : DbContext
    {
        public DbSet<PlayerEntity> Players { get; set; }
        public DbSet<TournamentEntity> Tournaments { get; set; }
        public DbSet<ResultEntity> Results { get; set; }

        public WtaContext(DbContextOptions<WtaContext> options) : base(options) { }
        public WtaContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=Tennis.db");
            }
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
            optionsBuilder.EnableSensitiveDataLogging(); // <-- AJOUTER CETTE LIGNE
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ResultEntity>()
                .HasMany(t => t.Tournaments)
                .WithMany(r => r.Results);

            modelBuilder.Entity<PlayerEntity>()
                        .HasMany(r => r.Results)
                        .WithMany(p => p.Players);
        }
    }
}