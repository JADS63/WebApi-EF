using System;
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
        private readonly string _baseUrl = "http://localhost:7001/api/v1/Players";

        public PlayerServiceDto()
        {
            _client = new ConsommationWebApi();
        }

        public async Task<IEnumerable<Player>> GetPlayersAsync()
        {
            string route = $"{_baseUrl}?Index=0&Count=10";
            Console.WriteLine($"[GET] Appel de l'endpoint: {route}");
            var response = await _client.GetFromRoute<ApiResponseDto<IEnumerable<PlayerDto>>>(route);
            if (response == null || response.Result == null)
            {
                Console.WriteLine("Erreur: Aucune réponse ou 'result' nulle pour GetPlayersAsync.");
                return Enumerable.Empty<Player>();
            }
            return response.Result.Select(dto => dto.ToPlayer());
        }

        public async Task<IEnumerable<Player>> GetPlayersAsync(int index, int count, int sortCriteria)
        {
            string route = $"{_baseUrl}?Index={index}&Count={count}&Sort={sortCriteria}";
            Console.WriteLine($"[GET] Appel de l'endpoint: {route}");
            var response = await _client.GetFromRoute<ApiResponseDto<IEnumerable<PlayerDto>>>(route);
            if (response == null || response.Result == null)
            {
                Console.WriteLine($"Erreur: Aucune réponse ou 'result' nulle pour GetPlayersAsync({index},{count},{sortCriteria}).");
                return Enumerable.Empty<Player>();
            }
            return response.Result.Select(dto => dto.ToPlayer());
        }

        public async Task<IEnumerable<Player>> GetPlayersByNameAsync(string name, int index, int count, int sortCriteria)
        {
            string route = $"{_baseUrl}/byName?name={name}&Index={index}&Count={count}&Sort={sortCriteria}";
            Console.WriteLine($"[GET] Appel de l'endpoint: {route}");
            var response = await _client.GetFromRoute<ApiResponseDto<IEnumerable<PlayerDto>>>(route);
            if (response == null || response.Result == null)
            {
                Console.WriteLine($"Erreur: Aucune réponse ou 'result' nulle pour GetPlayersByNameAsync('{name}',{index},{count},{sortCriteria}).");
                return Enumerable.Empty<Player>();
            }
            return response.Result.Select(dto => dto.ToPlayer());
        }

        public async Task<IEnumerable<Player>> GetPlayersByNationalityAsync(string nationality, int index, int count, int sortCriteria)
        {
            string route = $"{_baseUrl}/byNationality?nationality={nationality}&Index={index}&Count={count}&Sort={sortCriteria}";
            Console.WriteLine($"[GET] Appel de l'endpoint: {route}");
            var response = await _client.GetFromRoute<ApiResponseDto<IEnumerable<PlayerDto>>>(route);
            if (response == null || response.Result == null)
            {
                Console.WriteLine($"Erreur: Aucune réponse ou 'result' nulle pour GetPlayersByNationalityAsync('{nationality}',{index},{count},{sortCriteria}).");
                return Enumerable.Empty<Player>();
            }
            return response.Result.Select(dto => dto.ToPlayer());
        }

        public async Task<Player> GetPlayerByIdAsync(int id)
        {
            string route = $"{_baseUrl}/{id}";
            Console.WriteLine($"[GET] Appel de l'endpoint: {route}");
            var response = await _client.GetFromRoute<ApiResponseDto<PlayerDto>>(route);
            if (response == null || response.Result == null)
            {
                Console.WriteLine($"Erreur: Aucune réponse ou 'result' nulle pour GetPlayerByIdAsync({id}).");
                return null;
            }
            return response.Result.ToPlayer();
        }

        public async Task<Player> AddPlayerAsync(Player player)
        {
            string route = _baseUrl;
            PlayerDto dto = player.ToPlayerDto();
            Console.WriteLine($"[POST] Envoi d'un nouveau joueur à: {route}");
            var result = await _client.PostItemAsync<PlayerDto, PlayerDto>(route, dto);
            if (result == null)
            {
                Console.WriteLine("Erreur: Aucune réponse valide reçue pour l'ajout.");
                return null;
            }
            Console.WriteLine("Réponse reçue pour l'ajout.");
            return result.ToPlayer();
        }

        public async Task<Player> UpdatePlayerAsync(int id, Player player)
        {
            // Pour correspondre à l'exemple curl, on utilise la query string pour l'ID
            string route = $"{_baseUrl}?id={id}";
            Console.WriteLine($"[PUT] Mise à jour du joueur {id} via: {route}");
            PlayerDto dto = player.ToPlayerDto();
            var response = await _client.PutItemAsync<PlayerDto, ApiResponseDto<PlayerDto>>(route, dto);
            if (response == null || response.Result == null)
            {
                Console.WriteLine($"Erreur: Aucune réponse ou 'result' nulle pour UpdatePlayerAsync({id}).");
                return null;
            }
            Console.WriteLine("Réponse reçue pour la mise à jour.");
            return response.Result.ToPlayer();
        }

        public async Task<bool> DeletePlayerAsync(int id)
        {
            string route = $"{_baseUrl}/{id}";
            Console.WriteLine($"[DELETE] Suppression du joueur avec l'ID: {id} via: {route}");
            return await _client.DeleteItemAsync(route);
        }

        public async Task<int> GetTotalCountAsync()
        {
            string route = $"{_baseUrl}/TotalCount";
            Console.WriteLine($"[GET] Appel de l'endpoint TotalCount: {route}");
            var response = await _client.GetFromRoute<ApiResponseDto<int>>(route);
            if (response == null)
            {
                Console.WriteLine("Erreur: Aucune réponse reçue pour GetTotalCountAsync.");
                return 0;
            }
            return response.Result;
        }

        public async Task<int> GetTotalCountByNameAsync(string name)
        {
            string route = $"{_baseUrl}/TotalCountByName?name={name}";
            Console.WriteLine($"[GET] Appel de l'endpoint TotalCountByName: {route}");
            var response = await _client.GetFromRoute<ApiResponseDto<int>>(route);
            if (response == null)
            {
                Console.WriteLine($"Erreur: Aucune réponse reçue pour GetTotalCountByNameAsync('{name}').");
                return 0;
            }
            return response.Result;
        }

        public async Task<int> GetTotalCountByNationalityAsync(string nationality)
        {
            string route = $"{_baseUrl}/TotalCountByNationality?nationality={nationality}";
            Console.WriteLine($"[GET] Appel de l'endpoint TotalCountByNationality: {route}");
            var response = await _client.GetFromRoute<ApiResponseDto<int>>(route);
            if (response == null)
            {
                Console.WriteLine($"Erreur: Aucune réponse reçue pour GetTotalCountByNationalityAsync('{nationality}').");
                return 0;
            }
            return response.Result;
        }
    }
}
