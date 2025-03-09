// StubbedContextLib/StubbedContext.cs
using ContextLib;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace StubbedContextLib
{
    public class StubbedContext : WtaContext
    {
        public StubbedContext(DbContextOptions<WtaContext> options) : base(options)
        {
            Database.EnsureCreated(); 
        }

        public StubbedContext()
        {
            Database.EnsureCreated(); 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("TennisInMemoryDb");
            }
            base.OnConfiguring(optionsBuilder); 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Données stubbées pour les joueuses
            modelBuilder.Entity<PlayerEntity>().HasData(
                new PlayerEntity { Id = 42, FirstName = "Venus", LastName = "Williams", BirthDate = new DateTime(1980, 6, 17), Height = 1.85f, Nationality = "USA", Handplay = HandplayEntity.Right },
                new PlayerEntity { Id = 43, FirstName = "Serena", LastName = "Williams", BirthDate = new DateTime(1981, 9, 26), Height = 1.75f, Nationality = "USA", Handplay = HandplayEntity.Right }
            );

            // Données stubbées pour les tournois
            modelBuilder.Entity<TournamentEntity>().HasData(
                new TournamentEntity { Id = 42, Name = "Australian Open", Year = 2024 },
                new TournamentEntity { Id = 43, Name = "Wimbledon", Year = 2024 }
            );

            // Données stubbées pour les résultats
            modelBuilder.Entity<ResultEntity>().HasData(
                new ResultEntity { Id = 42, Result = Result.Final },
                new ResultEntity { Id = 43, Result = Result.Semifinal }
            );
        }
    }
}