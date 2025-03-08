using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Dto;

namespace Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Dto;

    namespace Services
    {


        public class PlayerServiceStub : IPlayerService
        {
            public  List<Player> _players;

            public PlayerServiceStub()
            {
                _players = new List<Player>
            {
                new Player { Id = 42, FirstName = "Aryna", LastName = "Sabalenka", BirthDate = new DateTime(1998, 5, 5), Height = 1.82, Nationality = "Belarus" },
                new Player { Id = 43, FirstName = "Iga", LastName = "Swiatek", BirthDate = new DateTime(2001, 5, 31), Height = 1.76, Nationality = "Poland" },
                new Player { Id = 44, FirstName = "Coco", LastName = "Gauff", BirthDate = new DateTime(2004, 3, 13), Height = 1.75, Nationality = "USA" },
                new Player { Id = 45, FirstName = "Jasmine", LastName = "Paolini", BirthDate = new DateTime(1996, 1, 4), Height = 1.63, Nationality = "Italy" },
                new Player { Id = 46, FirstName = "Qinwen", LastName = "Zheng", BirthDate = new DateTime(2002, 10, 8), Height = 1.78, Nationality = "China" },
                new Player { Id = 47, FirstName = "Elena", LastName = "Rybakina", BirthDate = new DateTime(1999, 6, 17), Height = 1.84, Nationality = "Kazakhstan" },
                new Player { Id = 48, FirstName = "Jessica", LastName = "Pegula", BirthDate = new DateTime(1994, 2, 24), Height = 1.70, Nationality = "USA" },
                new Player { Id = 49, FirstName = "Emma", LastName = "Navarro", BirthDate = new DateTime(2001, 5, 18), Height = 1.70, Nationality = "USA" }
            };
            }

            public Task<IEnumerable<Player>> GetPlayersAsync() =>
                Task.FromResult(_players.AsEnumerable());

            public Task<IEnumerable<Player>> GetPlayersAsync(int index, int count, int sortCriteria)
            {
                var sorted = ApplySorting(_players, sortCriteria);
                var result = sorted.Skip(index * count).Take(count);
                return Task.FromResult(result);
            }

            public Task<IEnumerable<Player>> GetPlayersByNameAsync(string name, int index, int count, int sortCriteria)
            {
                var filtered = _players.Where(p =>
                    (!string.IsNullOrEmpty(p.FirstName) && p.FirstName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0)
                    || (!string.IsNullOrEmpty(p.LastName) && p.LastName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0));
                var sorted = ApplySorting(filtered, sortCriteria);
                var result = sorted.Skip(index * count).Take(count);
                return Task.FromResult(result);
            }

            public Task<IEnumerable<Player>> GetPlayersByNationalityAsync(string nationality, int index, int count, int sortCriteria)
            {
                var filtered = _players.Where(p =>
                    !string.IsNullOrEmpty(p.Nationality) && p.Nationality.IndexOf(nationality, StringComparison.OrdinalIgnoreCase) >= 0);
                var sorted = ApplySorting(filtered, sortCriteria);
                var result = sorted.Skip(index * count).Take(count);
                return Task.FromResult(result);
            }

            public Task<Player> GetPlayerByIdAsync(int id)
            {
                var player = _players.FirstOrDefault(p => p.Id == id);
                return Task.FromResult(player);
            }

            public Task<Player> AddPlayerAsync(Player player)
            {
                int newId = _players.Max(p => p.Id) + 1;
                player.Id = newId;
                _players.Add(player);
                return Task.FromResult(player);
            }

            public Task<Player> UpdatePlayerAsync(int id, Player player)
            {
                var existing = _players.FirstOrDefault(p => p.Id == id);
                if (existing == null)
                    return Task.FromResult<Player>(null);
                existing.FirstName = player.FirstName;
                existing.LastName = player.LastName;
                existing.BirthDate = player.BirthDate;
                existing.Height = player.Height;
                existing.Nationality = player.Nationality;
                return Task.FromResult(existing);
            }

            public Task<bool> DeletePlayerAsync(int id)
            {
                var player = _players.FirstOrDefault(p => p.Id == id);
                if (player == null)
                    return Task.FromResult(false);
                _players.Remove(player);
                return Task.FromResult(true);
            }

            public Task<int> GetTotalCountAsync() =>
                Task.FromResult(_players.Count);

            public Task<int> GetTotalCountByNameAsync(string name)
            {
                int count = _players.Count(p =>
                    (!string.IsNullOrEmpty(p.FirstName) && p.FirstName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0)
                    || (!string.IsNullOrEmpty(p.LastName) && p.LastName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0));
                return Task.FromResult(count);
            }

            public Task<int> GetTotalCountByNationalityAsync(string nationality)
            {
                int count = _players.Count(p =>
                    !string.IsNullOrEmpty(p.Nationality) && p.Nationality.IndexOf(nationality, StringComparison.OrdinalIgnoreCase) >= 0);
                return Task.FromResult(count);
            }

            private IEnumerable<Player> ApplySorting(IEnumerable<Player> players, int sortCriteria)
            {
                switch ((SortCriteria)sortCriteria)
                {
                    case SortCriteria.ByNameThenFirstName:
                        return players.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
                    case SortCriteria.ByNameThenFirstNameDesc:
                        return players.OrderByDescending(p => p.LastName).ThenByDescending(p => p.FirstName);
                    case SortCriteria.ByNationality:
                        return players.OrderBy(p => p.Nationality);
                    case SortCriteria.ByNationalityDesc:
                        return players.OrderByDescending(p => p.Nationality);
                    case SortCriteria.ByBirthDate:
                        return players.OrderBy(p => p.BirthDate);
                    case SortCriteria.ByBirthDateDesc:
                        return players.OrderByDescending(p => p.BirthDate);
                    default:
                        return players.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
                }
            }
        }
    }

}
