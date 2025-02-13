using Entities;
using ContextLib;
using Microsoft.EntityFrameworkCore;
namespace StubbedContextLib
{
    public class StubbedContext : WtaContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerEntity>().HasData(
            
                new PlayerEntity
                {
                    Id = 42,
                    FirstName = "Aryna",
                    LastName = "Sabalenka",
                    BirthDate = new DateTime(1998, 5, 5),
                    Height = 1.82f,
                    Nationality = "Belarus",
                    Handplay = HandplayEntity.LeftAndRight
                },
                new PlayerEntity
                {
                    Id = 43,
                    FirstName = "Iga",
                    LastName = "Swiatek",
                    BirthDate = new DateTime(2001, 5, 31),
                    Height = 1.76f,
                    Nationality = "Poland",
                    Handplay = HandplayEntity.LeftAndRight
                },
                new PlayerEntity
                {
                    Id = 44,
                    FirstName = "Coco",
                    LastName = "Gauff",
                    BirthDate = new DateTime(2004, 3, 13),
                    Height = 1.75f,
                    Nationality = "USA",
                    Handplay = HandplayEntity.LeftAndRight
                },
                new PlayerEntity
                {
                    Id = 45,
                    FirstName = "Jasmine",
                    LastName = "Paolini",
                    BirthDate = new DateTime(1996, 1, 4),
                    Height = 1.63f,
                    Nationality = "Italy",
                    Handplay = HandplayEntity.LeftAndRight
                },
                new PlayerEntity
                {
                    Id = 46,
                    FirstName = "Qinwen",
                    LastName = "Zheng",
                    BirthDate = new DateTime(2002, 10, 8),
                    Height = 1.78f,
                    Nationality = "China",
                    Handplay = HandplayEntity.LeftAndRight
                },
                new PlayerEntity
                {
                    Id = 47,
                    FirstName = "Elena",
                    LastName = "Rybakina",
                    BirthDate = new DateTime(1999, 6, 17),
                    Height = 1.84f,
                    Nationality = "Kazakhstan",
                    Handplay = HandplayEntity.LeftAndRight
                },
                new PlayerEntity
                {
                    Id = 48,
                    FirstName = "Jessica",
                    LastName = "Pegula",
                    BirthDate = new DateTime(1994, 2, 24),
                    Height = 1.7f,
                    Nationality = "USA",
                    Handplay = HandplayEntity.LeftAndRight
                },
                new PlayerEntity
                {
                    Id = 49,
                    FirstName = "Emma",
                    LastName = "Navarro",
                    BirthDate = new DateTime(2001, 5, 18),
                    Height = 1.7f,
                    Nationality = "USA",
                    Handplay = HandplayEntity.LeftAndRight
                }
            );
        }
    }
}
