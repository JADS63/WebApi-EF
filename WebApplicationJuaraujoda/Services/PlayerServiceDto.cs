using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dto;
using Entities;
using Extensions;
using WebApiUtilisation;

namespace Services
{
    public class PlayerServiceDto : IPlayerService
    {
        private readonly ConsommationWebApi _client;
        private readonly string _baseUrl = "https://localhost:7001/api/players";


        public PlayerServiceDto()
        {
            _client = new ConsommationWebApi();
        }

        public async Task<IEnumerable<Player>> GetPlayersAsync()
        {
            string route = $"{_baseUrl}?index=0&count=10";
            IEnumerable<PlayerDto> dtos = await _client.GetFromRoute<IEnumerable<PlayerDto>>(route);
            return dtos.Select(dto => dto.ToPlayer());
        }

        public async Task<IEnumerable<Player>> GetPlayersAsync(int index, int count, int sortCriteria)
        {
            string route = $"{_baseUrl}?index={index}&count={count}&sort={sortCriteria}";
            IEnumerable<PlayerDto> dtos = await _client.GetFromRoute<IEnumerable<PlayerDto>>(route);
            return dtos.Select(dto => dto.ToPlayer());
        }

        public async Task<IEnumerable<Player>> GetPlayersByNameAsync(string name, int index, int count, int sortCriteria)
        {
            string route = $"{_baseUrl}/byName?name={name}&index={index}&count={count}&sort={sortCriteria}";
            IEnumerable<PlayerDto> dtos = await _client.GetFromRoute<IEnumerable<PlayerDto>>(route);
            return dtos.Select(dto => dto.ToPlayer());
        }

        public async Task<IEnumerable<Player>> GetPlayersByNationalityAsync(string nationality, int index, int count, int sortCriteria)
        {
            string route = $"{_baseUrl}/byNationality?nationality={nationality}&index={index}&count={count}&sort={sortCriteria}";
            IEnumerable<PlayerDto> dtos = await _client.GetFromRoute<IEnumerable<PlayerDto>>(route);
            return dtos.Select(dto => dto.ToPlayer());
        }

        public async Task<Player> GetPlayerByIdAsync(int id)
        {
            string route = $"{_baseUrl}/{id}";
            PlayerDto dto = await _client.GetFromRoute<PlayerDto>(route);
            return dto.ToPlayer();
        }

        public async Task<Player> AddPlayerAsync(Player player)
        {
            string route = _baseUrl;
            PlayerDto dto = player.ToPlayerDto();
            PlayerDto? result = await _client.PostItemAsync(route, dto);
            return result.ToPlayer();
        }

        public async Task<Player> UpdatePlayerAsync(int id, Player player)
        {
            string route = $"{_baseUrl}/{id}";
            PlayerDto dto = player.ToPlayerDto();
            PlayerDto? result = await _client.PutItemAsync(route, dto);
            return result.ToPlayer();
        }

        public async Task<bool> DeletePlayerAsync(int id)
        {
            string route = $"{_baseUrl}/{id}";
            return await _client.DeleteItemAsync(route);
        }

        public async Task<int> GetTotalCountAsync()
        {
            // Exemple : appel à une route spécifique pour récupérer le compte total
            string route = $"{_baseUrl}/totalCount";
            int count = await _client.GetFromRoute<int>(route);
            return count;
        }

        public async Task<int> GetTotalCountByNameAsync(string name)
        {
            string route = $"{_baseUrl}/totalCountByName?name={name}";
            int count = await _client.GetFromRoute<int>(route);
            return count;
        }

        public async Task<int> GetTotalCountByNationalityAsync(string nationality)
        {
            string route = $"{_baseUrl}/totalCountByNationality?nationality={nationality}";
            int count = await _client.GetFromRoute<int>(route);
            return count;
        }
    }
}
