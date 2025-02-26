using Entities;
using System.Collections.Generic;

namespace Services
{
    public interface IPlayerService
    {
        IEnumerable<Player> GetPlayers();
        IEnumerable<Player> GetPlayers(int index, int count, int sortCriteria);
        IEnumerable<Player> GetPlayersByName(string name, int index, int count, int sortCriteria);
        IEnumerable<Player> GetPlayersByNationality(string nationality, int index, int count, int sortCriteria);
        Player GetPlayerById(int id);
        Player AddPlayer(Player player);
        Player UpdatePlayer(int id, Player player);
        bool DeletePlayer(int id);
        int GetTotalCount();
        int GetTotalCountByName(string name);
        int GetTotalCountByNationality(string nationality);
    }
}
