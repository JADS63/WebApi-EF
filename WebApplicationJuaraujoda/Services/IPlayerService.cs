using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface IPlayerService
    {
        Task<IEnumerable<Player>> GetPlayersAsync();
        Task<IEnumerable<Player>> GetPlayersAsync(int index, int count, Entities.SortCriteria sortCriteria);
        Task<IEnumerable<Player>> GetPlayersByNameAsync(string name, int index, int count, Entities.SortCriteria sortCriteria);
        Task<IEnumerable<Player>> GetPlayersByNationalityAsync(string nationality, int index, int count, Entities.SortCriteria sortCriteria);
        Task<Player?> GetPlayerByIdAsync(int id);
        Task<Player?> AddPlayerAsync(Player player);
        Task<Player?> UpdatePlayerAsync(int id, Player player);
        Task<bool> DeletePlayerAsync(int id);
        Task<int> GetTotalCountAsync();
        Task<int> GetTotalCountByNameAsync(string name);
        Task<int> GetTotalCountByNationalityAsync(string nationality);
    }
}