using Entities;
using Stub;
using Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class PlayerServiceStub : IPlayerService
    {
        private readonly List<Player> _players;

        public PlayerServiceStub()
        {
            _players = StubTennis.GetPlayers();
        }

        public IEnumerable<Player> GetPlayers() => _players;

        public IEnumerable<Player> GetPlayers(int index, int count, int sortCriteria)
        {
            var sorted = ApplySorting(_players, sortCriteria);
            return sorted.Skip(index * count).Take(count);
        }

        public IEnumerable<Player> GetPlayersByName(string name, int index, int count, int sortCriteria)
        {
            var filtered = _players.Where(p =>
                (!string.IsNullOrEmpty(p.FirstName) && p.FirstName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0)
                || (!string.IsNullOrEmpty(p.LastName) && p.LastName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0));
            var sorted = ApplySorting(filtered, sortCriteria);
            return sorted.Skip(index * count).Take(count);
        }

        public IEnumerable<Player> GetPlayersByNationality(string nationality, int index, int count, int sortCriteria)
        {
            var filtered = _players.Where(p =>
                !string.IsNullOrEmpty(p.Nationality) && p.Nationality.IndexOf(nationality, StringComparison.OrdinalIgnoreCase) >= 0);
            var sorted = ApplySorting(filtered, sortCriteria);
            return sorted.Skip(index * count).Take(count);
        }

        public Player GetPlayerById(int id) => _players.FirstOrDefault(p => p.Id == id);

        public Player AddPlayer(Player player)
        {
            int newId = _players.Max(p => p.Id) + 1;
            player.Id = newId;
            _players.Add(player);
            return player;
        }

        public Player UpdatePlayer(int id, Player player)
        {
            var existing = _players.FirstOrDefault(p => p.Id == id);
            if (existing == null)
                return null;
            existing.FirstName = player.FirstName;
            existing.LastName = player.LastName;
            existing.Height = player.Height;
            existing.BirthDate = player.BirthDate;
            existing.HandPlay = player.HandPlay;
            existing.Nationality = player.Nationality;
            return existing;
        }

        public bool DeletePlayer(int id)
        {
            var player = _players.FirstOrDefault(p => p.Id == id);
            if (player == null)
                return false;
            _players.Remove(player);
            return true;
        }

        public int GetTotalCount() => _players.Count;
        public int GetTotalCountByName(string name) =>
            _players.Count(p =>
                (!string.IsNullOrEmpty(p.FirstName) && p.FirstName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0)
                || (!string.IsNullOrEmpty(p.LastName) && p.LastName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0));
        public int GetTotalCountByNationality(string nationality) =>
            _players.Count(p => !string.IsNullOrEmpty(p.Nationality) && p.Nationality.IndexOf(nationality, StringComparison.OrdinalIgnoreCase) >= 0);

        private IEnumerable<Player> ApplySorting(IEnumerable<Player> players, int sortCriteria)
        {
            switch (sortCriteria)
            {
                case (int)Dto.SortCriteria.ByNameThenFirstName:
                    return players.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
                case (int)Dto.SortCriteria.ByNameThenFirstNameDesc:
                    return players.OrderByDescending(p => p.LastName).ThenByDescending(p => p.FirstName);
                case (int)Dto.SortCriteria.ByNationality:
                    return players.OrderBy(p => p.Nationality);
                case (int)Dto.SortCriteria.ByNationalityDesc:
                    return players.OrderByDescending(p => p.Nationality);
                case (int)Dto.SortCriteria.ByBirthDate:
                    return players.OrderBy(p => p.BirthDate);
                case (int)Dto.SortCriteria.ByBirthDateDesc:
                    return players.OrderByDescending(p => p.BirthDate);
                default:
                    return players.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
            }
        }
    }
}
