using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;

namespace Services
{
    public class PlayerServiceModel : IPlayerService
    {
        private readonly PlayerServiceDto _serviceDto;

        public PlayerServiceModel()
        {
            _serviceDto = new PlayerServiceDto();
        }

        public Task<IEnumerable<Player>> GetPlayersAsync() =>
            _serviceDto.GetPlayersAsync();

        public Task<IEnumerable<Player>> GetPlayersAsync(int index, int count, int sortCriteria) =>
            _serviceDto.GetPlayersAsync(index, count, sortCriteria);

        public Task<IEnumerable<Player>> GetPlayersByNameAsync(string name, int index, int count, int sortCriteria) =>
            _serviceDto.GetPlayersByNameAsync(name, index, count, sortCriteria);

        public Task<IEnumerable<Player>> GetPlayersByNationalityAsync(string nationality, int index, int count, int sortCriteria) =>
            _serviceDto.GetPlayersByNationalityAsync(nationality, index, count, sortCriteria);

        public Task<Player> GetPlayerByIdAsync(int id) =>
            _serviceDto.GetPlayerByIdAsync(id);

        public Task<Player> AddPlayerAsync(Player player) =>
            _serviceDto.AddPlayerAsync(player);

        public Task<Player> UpdatePlayerAsync(int id, Player player) =>
            _serviceDto.UpdatePlayerAsync(id, player);

        public Task<bool> DeletePlayerAsync(int id) =>
            _serviceDto.DeletePlayerAsync(id);

        public Task<int> GetTotalCountAsync() =>
            _serviceDto.GetTotalCountAsync();

        public Task<int> GetTotalCountByNameAsync(string name) =>
            _serviceDto.GetTotalCountByNameAsync(name);

        public Task<int> GetTotalCountByNationalityAsync(string nationality) =>
            _serviceDto.GetTotalCountByNationalityAsync(nationality);
    }
}
