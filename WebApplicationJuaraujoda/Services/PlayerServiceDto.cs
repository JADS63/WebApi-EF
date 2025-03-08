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
        private readonly string _baseUrl;

        public PlayerServiceDto()
        {
            _baseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://localhost:7001/api/v1/Players";
            _client = new ConsommationWebApi { Client = { BaseAddress = new Uri(_baseUrl) } };
        }

        public async Task<IEnumerable<Player>> GetPlayersAsync()
        {
            string route = $"?Index=0&Count=10";
            Console.WriteLine($"[GET] Appel de l'endpoint: {_baseUrl}{route}");
            var response = await _client.GetFromRoute<ApiResponseDto<IEnumerable<PlayerDto>>>(route);

            if (response == null || response.Result == null)
            {
                // Lance une exception personnalisée au lieu de Console.WriteLine
                throw new Exception("Aucune réponse ou 'result' null pour GetPlayersAsync.");
            }

            return response.Result.Select(dto => dto.ToPlayer());
        }

        public async Task<IEnumerable<Player>> GetPlayersAsync(int index, int count, Entities.SortCriteria sortCriteria)
        {
            string route = $"?Index={index}&Count={count}&Sort={(int)sortCriteria}";
            Console.WriteLine($"[GET] Appel de l'endpoint: {_baseUrl}{route}");
            var response = await _client.GetFromRoute<ApiResponseDto<IEnumerable<PlayerDto>>>(route);
            if (response == null || response.Result == null)
            {
                throw new Exception($"Aucune réponse ou 'result' nulle pour GetPlayersAsync({index},{count},{sortCriteria}).");
            }
            return response.Result.Select(dto => dto.ToPlayer());
        }

        public async Task<IEnumerable<Player>> GetPlayersByNameAsync(string name, int index, int count, Entities.SortCriteria sortCriteria)
        {
            string route = $"/byName?name={name}&Index={index}&Count={count}&Sort={(int)sortCriteria}";
            Console.WriteLine($"[GET] Appel de l'endpoint: {_baseUrl}{route}");
            var response = await _client.GetFromRoute<ApiResponseDto<IEnumerable<PlayerDto>>>(route);
            if (response == null || response.Result == null)
            {
                throw new Exception($"Aucune réponse ou 'result' nulle pour GetPlayersByNameAsync('{name}',{index},{count},{sortCriteria}).");
            }
            return response.Result.Select(dto => dto.ToPlayer());
        }

        public async Task<IEnumerable<Player>> GetPlayersByNationalityAsync(string nationality, int index, int count, Entities.SortCriteria sortCriteria)
        {
            string route = $"/byNationality?nationality={nationality}&Index={index}&Count={count}&Sort={(int)sortCriteria}";
            Console.WriteLine($"[GET] Appel de l'endpoint: {_baseUrl}{route}");
            var response = await _client.GetFromRoute<ApiResponseDto<IEnumerable<PlayerDto>>>(route);
            if (response == null || response.Result == null)
            {
                throw new Exception($"Aucune réponse ou 'result' nulle pour GetPlayersByNationalityAsync('{nationality}',{index},{count},{sortCriteria}).");
            }
            return response.Result.Select(dto => dto.ToPlayer());
        }

        public async Task<Player?> GetPlayerByIdAsync(int id)
        {
            string route = $"/{id}";
            Console.WriteLine($"[GET] Appel de l'endpoint: {_baseUrl}{route}");
            var response = await _client.GetFromRoute<ApiResponseDto<PlayerDto>>(route);
            if (response == null || response.Result == null)
            {
                Console.WriteLine($"Erreur: Aucune réponse ou 'result' nulle pour GetPlayerByIdAsync({id}).");
                return null;
            }
            return response.Result.ToPlayer();
        }

        public async Task<Player?> AddPlayerAsync(Player player)
        {
            string route = "";
            PlayerDto dto = player.ToPlayerDto();
            Console.WriteLine($"[POST] Envoi d'un nouveau joueur à: {_baseUrl}{route}");
            var result = await _client.PostItemAsync<PlayerDto, PlayerDto>(route, dto);
            if (result == null)
            {
                Console.WriteLine("Erreur: Aucune réponse valide reçue pour l'ajout.");
                return null;
            }
            Console.WriteLine("Réponse reçue pour l'ajout.");
            return result.ToPlayer();
        }

        public async Task<Player?> UpdatePlayerAsync(int id, Player player)
        {
            string route = $"?id={id}";
            Console.WriteLine($"[PUT] Mise à jour du joueur {id} via: {_baseUrl}{route}");
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
            string route = $"/{id}";
            Console.WriteLine($"[DELETE] Suppression du joueur avec l'ID: {id} via: {_baseUrl}{route}");
            return await _client.DeleteItemAsync(route);
        }

        public async Task<int> GetTotalCountAsync()
        {
            string route = $"/TotalCount";
            Console.WriteLine($"[GET] Appel de l'endpoint TotalCount: {_baseUrl}{route}");
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
            string route = $"/TotalCountByName?name={name}";
            Console.WriteLine($"[GET] Appel de l'endpoint TotalCountByName: {_baseUrl}{route}");
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
            string route = $"/TotalCountByNationality?nationality={nationality}";
            Console.WriteLine($"[GET] Appel de l'endpoint TotalCountByNationality: {_baseUrl}{route}");
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