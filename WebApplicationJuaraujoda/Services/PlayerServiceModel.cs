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

        public async Task<IEnumerable<Player>> GetPlayersAsync()
            => await _serviceDto.GetPlayersAsync();

        public async Task<IEnumerable<Player>> GetPlayersAsync(int index, int count, int sortCriteria)
            => await _serviceDto.GetPlayersAsync(index, count, sortCriteria);

        public async Task<IEnumerable<Player>> GetPlayersByNameAsync(string name, int index, int count, int sortCriteria)
            => await _serviceDto.GetPlayersByNameAsync(name, index, count, sortCriteria);

        public async Task<IEnumerable<Player>> GetPlayersByNationalityAsync(string nationality, int index, int count, int sortCriteria)
            => await _serviceDto.GetPlayersByNationalityAsync(nationality, index, count, sortCriteria);

        public async Task<Player> GetPlayerByIdAsync(int id)
            => await _serviceDto.GetPlayerByIdAsync(id);

        public async Task<Player> AddPlayerAsync(Player player)
            => await _serviceDto.AddPlayerAsync(player);

        public async Task<Player> UpdatePlayerAsync(int id, Player player)
            => await _serviceDto.UpdatePlayerAsync(id, player);

        public async Task<bool> DeletePlayerAsync(int id)
            => await _serviceDto.DeletePlayerAsync(id);

        public async Task<int> GetTotalCountAsync()
            => await _serviceDto.GetTotalCountAsync();

        public async Task<int> GetTotalCountByNameAsync(string name)
            => await _serviceDto.GetTotalCountByNameAsync(name);

        public async Task<int> GetTotalCountByNationalityAsync(string nationality)
            => await _serviceDto.GetTotalCountByNationalityAsync(nationality);
    }
}
