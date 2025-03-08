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
        private readonly string _baseUrl; // Stocke l'URL de base

        public PlayerServiceDto()
        {
            // 1. Lit la variable d'environnement API_BASE_URL.
            // 2. Si elle n'est PAS définie (environnement local), utilise une valeur par défaut.
            //    Cela vous permet de tester en local sans avoir à définir la variable.
            _baseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://localhost:7001/api/v1/Players";

            // Initialise le client HTTP avec l'URL de base.
            _client = new ConsommationWebApi { Client = { BaseAddress = new Uri(_baseUrl) } };
        }

        // GET: api/v1/Players?Index=0&Count=10&Sort=0
        public async Task<IEnumerable<Player>> GetPlayersAsync()
        {
            // Construit la route *relative*.  _baseUrl est déjà pris en compte.
            string route = $"?Index=0&Count=10";
            Console.WriteLine($"[GET] Appel de l'endpoint: {_baseUrl}{route}"); // Log complet
            var response = await _client.GetFromRoute<ApiResponseDto<IEnumerable<PlayerDto>>>(route);

            // Gestion des erreurs (important!)
            if (response == null || response.Result == null)
            {
                Console.WriteLine("Erreur: Aucune réponse ou 'result' nulle pour GetPlayersAsync.");
                return Enumerable.Empty<Player>(); // Retourne une liste vide en cas d'erreur
            }

            // Mappe les DTOs vers les entités métier.
            return response.Result.Select(dto => dto.ToPlayer());
        }

        // GET: api/v1/Players?Index=0&Count=10&Sort=0
        public async Task<IEnumerable<Player>> GetPlayersAsync(int index, int count, int sortCriteria)
        {
            string route = $"?Index={index}&Count={count}&Sort={sortCriteria}";
            Console.WriteLine($"[GET] Appel de l'endpoint: {_baseUrl}{route}");
            var response = await _client.GetFromRoute<ApiResponseDto<IEnumerable<PlayerDto>>>(route);
            if (response == null || response.Result == null)
            {
                Console.WriteLine($"Erreur: Aucune réponse ou 'result' nulle pour GetPlayersAsync({index},{count},{sortCriteria}).");
                return Enumerable.Empty<Player>();
            }
            return response.Result.Select(dto => dto.ToPlayer());
        }

        // GET: api/v1/Players/byName?name=John&Index=0&Count=10&Sort=0
        public async Task<IEnumerable<Player>> GetPlayersByNameAsync(string name, int index, int count, int sortCriteria)
        {
            string route = $"/byName?name={name}&Index={index}&Count={count}&Sort={sortCriteria}";
            Console.WriteLine($"[GET] Appel de l'endpoint: {_baseUrl}{route}");
            var response = await _client.GetFromRoute<ApiResponseDto<IEnumerable<PlayerDto>>>(route);
            if (response == null || response.Result == null)
            {
                Console.WriteLine($"Erreur: Aucune réponse ou 'result' nulle pour GetPlayersByNameAsync('{name}',{index},{count},{sortCriteria}).");
                return Enumerable.Empty<Player>();
            }
            return response.Result.Select(dto => dto.ToPlayer());
        }

        // GET: api/v1/Players/byNationality?nationality=USA&Index=0&Count=10&Sort=0
        public async Task<IEnumerable<Player>> GetPlayersByNationalityAsync(string nationality, int index, int count, int sortCriteria)
        {
            string route = $"/byNationality?nationality={nationality}&Index={index}&Count={count}&Sort={sortCriteria}";
            Console.WriteLine($"[GET] Appel de l'endpoint: {_baseUrl}{route}");
            var response = await _client.GetFromRoute<ApiResponseDto<IEnumerable<PlayerDto>>>(route);
            if (response == null || response.Result == null)
            {
                Console.WriteLine($"Erreur: Aucune réponse ou 'result' nulle pour GetPlayersByNationalityAsync('{nationality}',{index},{count},{sortCriteria}).");
                return Enumerable.Empty<Player>();
            }
            return response.Result.Select(dto => dto.ToPlayer());
        }

        // GET: api/v1/Players/42
        public async Task<Player> GetPlayerByIdAsync(int id)
        {
            string route = $"/{id}";
            Console.WriteLine($"[GET] Appel de l'endpoint: {_baseUrl}{route}");
            var response = await _client.GetFromRoute<ApiResponseDto<PlayerDto>>(route);
            if (response == null || response.Result == null)
            {
                Console.WriteLine($"Erreur: Aucune réponse ou 'result' nulle pour GetPlayerByIdAsync({id}).");
                return null; // Retourne null en cas d'erreur
            }
            return response.Result.ToPlayer();
        }

        // POST: api/v1/Players
        public async Task<Player> AddPlayerAsync(Player player)
        {
            string route = ""; // La route de base est déjà dans _baseUrl
            PlayerDto dto = player.ToPlayerDto();  // Convertit l'entité Player en PlayerDto
            Console.WriteLine($"[POST] Envoi d'un nouveau joueur à: {_baseUrl}{route}");
            var result = await _client.PostItemAsync<PlayerDto, PlayerDto>(route, dto); // Envoie le DTO
            if (result == null)
            {
                Console.WriteLine("Erreur: Aucune réponse valide reçue pour l'ajout.");
                return null;
            }
            Console.WriteLine("Réponse reçue pour l'ajout.");
            return result.ToPlayer(); // Convertit le PlayerDto résultant en Player
        }

        // PUT: api/v1/Players?id=50
        public async Task<Player> UpdatePlayerAsync(int id, Player player)
        {
            string route = $"?id={id}";
            Console.WriteLine($"[PUT] Mise à jour du joueur {id} via: {_baseUrl}{route}");
            PlayerDto dto = player.ToPlayerDto(); // Convertit en DTO
            var response = await _client.PutItemAsync<PlayerDto, ApiResponseDto<PlayerDto>>(route, dto); // Envoie le DTO
            if (response == null || response.Result == null)
            {
                Console.WriteLine($"Erreur: Aucune réponse ou 'result' nulle pour UpdatePlayerAsync({id}).");
                return null;
            }
            Console.WriteLine("Réponse reçue pour la mise à jour.");
            return response.Result.ToPlayer(); // Convertit le DTO résultant en Player
        }

        // DELETE: api/v1/Players/50
        public async Task<bool> DeletePlayerAsync(int id)
        {
            string route = $"/{id}";
            Console.WriteLine($"[DELETE] Suppression du joueur avec l'ID: {id} via: {_baseUrl}{route}");
            return await _client.DeleteItemAsync(route); // Supprime et retourne le booléen de succès
        }

        // GET: api/v1/Players/TotalCount
        public async Task<int> GetTotalCountAsync()
        {
            string route = $"/TotalCount";
            Console.WriteLine($"[GET] Appel de l'endpoint TotalCount: {_baseUrl}{route}");
            var response = await _client.GetFromRoute<ApiResponseDto<int>>(route);
            if (response == null || response.Result == 0)
            {
                Console.WriteLine("Erreur: Aucune réponse reçue pour GetTotalCountAsync.");
                return 0; // Retourne 0 en cas d'erreur
            }
            return response.Result;
        }

        // GET: api/v1/Players/TotalCountByName?name=John
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

        // GET: api/v1/Players/TotalCountByNationality?nationality=USA
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