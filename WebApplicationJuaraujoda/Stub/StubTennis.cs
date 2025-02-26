using Entities;
using System;
using System.Collections.Generic;

namespace Stub
{
    public class StubTennis
    {
        public static List<Player> GetPlayers()
        {
            return new List<Player>
            {
                new Player
                {
                    Id = 42,
                    FirstName = "Aryna",
                    LastName = "Sabalenka",
                    BirthDate = new DateTime(1998, 5, 5),
                    Height = 1.82,
                    Nationality = "Belarus",
                    HandPlay = HandPlay.LeftAndRight
                },
                new Player
                {
                    Id = 43,
                    FirstName = "Iga",
                    LastName = "Swiatek",
                    BirthDate = new DateTime(2001, 5, 31),
                    Height = 1.76,
                    Nationality = "Poland",
                    HandPlay = HandPlay.LeftAndRight
                },
                new Player
                {
                    Id = 44,
                    FirstName = "Coco",
                    LastName = "Gauff",
                    BirthDate = new DateTime(2004, 3, 13),
                    Height = 1.75,
                    Nationality = "USA",
                    HandPlay = HandPlay.LeftAndRight
                },
                new Player
                {
                    Id = 45,
                    FirstName = "Jasmine",
                    LastName = "Paolini",
                    BirthDate = new DateTime(1996, 1, 4),
                    Height = 1.63,
                    Nationality = "Italy",
                    HandPlay = HandPlay.LeftAndRight
                },
                new Player
                {
                    Id = 46,
                    FirstName = "Qinwen",
                    LastName = "Zheng",
                    BirthDate = new DateTime(2002, 10, 8),
                    Height = 1.78,
                    Nationality = "China",
                    HandPlay = HandPlay.LeftAndRight
                },
                new Player
                {
                    Id = 47,
                    FirstName = "Elena",
                    LastName = "Rybakina",
                    BirthDate = new DateTime(1999, 6, 17),
                    Height = 1.84,
                    Nationality = "Kazakhstan",
                    HandPlay = HandPlay.LeftAndRight
                },
                new Player
                {
                    Id = 48,
                    FirstName = "Jessica",
                    LastName = "Pegula",
                    BirthDate = new DateTime(1994, 2, 24),
                    Height = 1.7,
                    Nationality = "USA",
                    HandPlay = HandPlay.LeftAndRight
                },
                new Player
                {
                    Id = 49,
                    FirstName = "Emma",
                    LastName = "Navarro",
                    BirthDate = new DateTime(2001, 5, 18),
                    Height = 1.7,
                    Nationality = "USA",
                    HandPlay = HandPlay.LeftAndRight
                }
            };
        }
    }
}
