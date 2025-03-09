using System;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Services;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IPlayerService playerService = new PlayerServiceModel();

            Console.WriteLine("=== Récupération de tous les joueurs ===");
            var players = await playerService.GetPlayersAsync();
            foreach (var player in players)
            {
                Console.WriteLine($"{player.Id} - {player.FirstName} {player.LastName}");
            }
            Console.WriteLine();

            if (players.Any())
            {
                Console.WriteLine("=== Récupération d'un joueur par ID ===");
                int firstId = players.First().Id;
                var playerById = await playerService.GetPlayerByIdAsync(firstId);
                if (playerById != null)
                {
                    Console.WriteLine($"{playerById.Id} - {playerById.FirstName} {playerById.LastName}");
                }
                else
                {
                    Console.WriteLine("Aucun joueur trouvé pour cet ID.");
                }
                Console.WriteLine();
            }

            Console.WriteLine("=== Ajout d'un nouveau joueur ===");
            var newPlayer = new Player
            {
                Id = 0,
                FirstName = "John",
                LastName = "Doe",
                BirthDate = DateTime.Now.AddYears(-25),
                Height = 180,
                Nationality = "USA"
            };

            var addedPlayer = await playerService.AddPlayerAsync(newPlayer);
            if (addedPlayer != null)
            {
                Console.WriteLine($"Nouveau joueur ajouté : {addedPlayer.Id} - {addedPlayer.FirstName} {addedPlayer.LastName}");
            }
            else
            {
                Console.WriteLine("L'ajout du joueur a échoué ou aucune donnée n'a été renvoyée par l'API.");
            }
            Console.WriteLine();

            if (addedPlayer != null)
            {
                Console.WriteLine("=== Mise à jour du joueur ajouté ===");
                addedPlayer.FirstName = "Johnny";
                var updatedPlayer = await playerService.UpdatePlayerAsync(addedPlayer.Id, addedPlayer);
                if (updatedPlayer != null)
                {
                    Console.WriteLine($"Joueur mis à jour : {updatedPlayer.Id} - {updatedPlayer.FirstName} {updatedPlayer.LastName}");
                }
                else
                {
                    Console.WriteLine("La mise à jour a échoué.");
                }
                Console.WriteLine();
            }

            Console.WriteLine("=== Nombre total de joueurs ===");
            int totalCount = await playerService.GetTotalCountAsync();
            Console.WriteLine($"Nombre total de joueurs : {totalCount}");
            Console.WriteLine();

            Console.WriteLine("=== Nombre total de joueurs avec le nom 'John' ===");
            int countByName = await playerService.GetTotalCountByNameAsync("John");
            Console.WriteLine($"Nombre de joueurs avec le nom 'John' : {countByName}");
            Console.WriteLine();

            Console.WriteLine("=== Nombre total de joueurs de nationalité 'USA' ===");
            int countByNationality = await playerService.GetTotalCountByNationalityAsync("USA");
            Console.WriteLine($"Nombre de joueurs de nationalité 'USA' : {countByNationality}");
            Console.WriteLine();

            Console.WriteLine("=== Récupération des joueurs par nom ('John') ===");
            var playersByName = await playerService.GetPlayersByNameAsync("John", 0, 10, 0);
            foreach (var p in playersByName)
            {
                Console.WriteLine($"{p.Id} - {p.FirstName} {p.LastName}");
            }
            Console.WriteLine();

            Console.WriteLine("=== Récupération des joueurs par nationalité ('USA') ===");
            var playersByNationality = await playerService.GetPlayersByNationalityAsync("USA", 0, 10, 0);
            foreach (var p in playersByNationality)
            {
                Console.WriteLine($"{p.Id} - {p.FirstName} {p.LastName}");
            }
            Console.WriteLine();

            if (addedPlayer != null)
            {
                Console.WriteLine("=== Suppression du joueur ajouté ===");
                bool deleted = await playerService.DeletePlayerAsync(addedPlayer.Id);
                if (deleted)
                {
                    Console.WriteLine($"Le joueur avec l'id {addedPlayer.Id} a été supprimé.");
                }
                else
                {
                    Console.WriteLine("La suppression du joueur a échoué.");
                }
                Console.WriteLine();
            }

            Console.WriteLine("=== Tests terminés ===");
        }
    }
}
