using System;
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

            var players = await playerService.GetPlayersAsync();
            foreach (var player in players)
            {
                Console.WriteLine($"{player.Id} - {player.FirstName} {player.LastName}");
            }

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
            Console.WriteLine($"Nouveau joueur ajouté : {addedPlayer.Id} - {addedPlayer.FirstName} {addedPlayer.LastName}");

        }
    }
}
