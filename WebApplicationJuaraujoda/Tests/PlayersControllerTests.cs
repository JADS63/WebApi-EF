using Xunit;
using WebApplicationJuaraujoda.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Entities;
using WtaApi.Mappers;
using Dto;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Services;
using System.Threading.Tasks;

namespace WebApplicationJuaraujoda.Tests
{
    // Stub générique implémentant IPlayerService
    public class PlayerServiceStub : IPlayerService
    {
        public List<Player> Players { get; set; } = new List<Player>();

        public Task<Player> AddPlayerAsync(Player player)
        {
            player.Id = 100; // Stub : définir un nouvel ID
            Players.Add(player);
            return Task.FromResult(player);
        }

        public Task<bool> DeletePlayerAsync(int id)
        {
            var player = Players.Find(p => p.Id == id);
            if (player != null)
            {
                Players.Remove(player);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<Player> GetPlayerByIdAsync(int id)
        {
            var player = Players.Find(p => p.Id == id);
            return Task.FromResult(player);
        }

        // Méthode publique utilisée pour récupérer la liste complète sous forme de List<Player>
        public Task<List<Player>> GetPlayersAsync()
        {
            return Task.FromResult(Players);
        }

        // Méthode publique pour la pagination retournant une List<Player>
        public Task<List<Player>> GetPlayersAsync(int index, int count, int criterium)
        {
            var result = Players.GetRange(index, Math.Min(count, Players.Count - index));
            return Task.FromResult(result);
        }

        public Task<List<Player>> GetPlayersByNameAsync(string name, int index, int count, int criterium)
        {
            var filtered = Players.FindAll(p =>
                p.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                p.LastName.Contains(name, StringComparison.OrdinalIgnoreCase));
            var result = filtered.GetRange(index, Math.Min(count, filtered.Count - index));
            return Task.FromResult(result);
        }

        public Task<int> GetTotalCountAsync()
        {
            return Task.FromResult(Players.Count);
        }

        public Task<int> GetTotalCountByNameAsync(string name)
        {
            var count = Players.FindAll(p =>
                p.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                p.LastName.Contains(name, StringComparison.OrdinalIgnoreCase)).Count;
            return Task.FromResult(count);
        }

        public Task<List<Player>> GetPlayersByNationalityAsync(string nationality, int index, int count, int criterium)
        {
            var filtered = Players.FindAll(p =>
                p.Nationality.Contains(nationality, StringComparison.OrdinalIgnoreCase));
            var result = filtered.GetRange(index, Math.Min(count, filtered.Count - index));
            return Task.FromResult(result);
        }

        public Task<int> GetTotalCountByNationalityAsync(string nationality)
        {
            var count = Players.FindAll(p =>
                p.Nationality.Contains(nationality, StringComparison.OrdinalIgnoreCase)).Count;
            return Task.FromResult(count);
        }

        public Task<Player> UpdatePlayerAsync(int id, Player player)
        {
            var existing = Players.Find(p => p.Id == id);
            if (existing != null)
            {
                existing.FirstName = player.FirstName;
                existing.LastName = player.LastName;
                existing.Height = player.Height;
                existing.BirthDate = player.BirthDate;
                existing.HandPlay = player.HandPlay;
                existing.Nationality = player.Nationality;
            }
            return Task.FromResult(existing);
        }

        // Implémentations explicites pour retourner IEnumerable<Player> conformément à l'interface

        Task<IEnumerable<Player>> IPlayerService.GetPlayersAsync()
        {
            // Utilise la méthode publique existante et la convertit en IEnumerable<Player>
            return Task.FromResult((IEnumerable<Player>)Players);
        }

        Task<IEnumerable<Player>> IPlayerService.GetPlayersAsync(int index, int count, int sortCriteria)
        {
            var result = Players.GetRange(index, Math.Min(count, Players.Count - index));
            return Task.FromResult((IEnumerable<Player>)result);
        }

        Task<IEnumerable<Player>> IPlayerService.GetPlayersByNameAsync(string name, int index, int count, int sortCriteria)
        {
            var filtered = Players.FindAll(p =>
                p.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                p.LastName.Contains(name, StringComparison.OrdinalIgnoreCase));
            var result = filtered.GetRange(index, Math.Min(count, filtered.Count - index));
            return Task.FromResult((IEnumerable<Player>)result);
        }

        Task<IEnumerable<Player>> IPlayerService.GetPlayersByNationalityAsync(string nationality, int index, int count, int sortCriteria)
        {
            var filtered = Players.FindAll(p =>
                p.Nationality.Contains(nationality, StringComparison.OrdinalIgnoreCase));
            var result = filtered.GetRange(index, Math.Min(count, filtered.Count - index));
            return Task.FromResult((IEnumerable<Player>)result);
        }
    }

    public class PlayersControllerStubTests
    {
        private readonly ILogger<PlayersController> _logger = NullLogger<PlayersController>.Instance;

        [Fact]
        public async Task GetPlayers_ReturnsOkResult_WithAllPlayers()
        {
            // Création d'un stub spécifique pour ce test et initialisation des données
            var stubService = new PlayerServiceStub();
            stubService.Players.AddRange(new List<Player>
            {
                new Player
                {
                    Id = 1,
                    FirstName = "Aryna",
                    LastName = "Sabalenka",
                    Height = 1.82,
                    BirthDate = new DateTime(1998, 5, 5),
                    HandPlay = Entities.HandPlay.Right,
                    Nationality = "Belarus"
                },
                new Player
                {
                    Id = 2,
                    FirstName = "Iga",
                    LastName = "Swiatek",
                    Height = 1.76,
                    BirthDate = new DateTime(2001, 5, 31),
                    HandPlay = Entities.HandPlay.Right,
                    Nationality = "Poland"
                }
            });
            var controller = new PlayersController(stubService, _logger);

            // Act : appel sans paramètres (donc sans pagination)
            var result = await controller.GetPlayers();
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            var response = okResult.Value as ApiResponseDto<List<PlayerDto>>;
            Assert.NotNull(response);
            Assert.Equal(1, response.Id); // id = 1 pour l'appel sans pagination
            Assert.Equal(2, response.Result.Count);
        }

        [Fact]
        public async Task GetPlayers_WithPagination_ReturnsOkResult_WithPaginatedPlayers()
        {
            // Création d'un stub spécifique pour ce test et initialisation des données
            var stubService = new PlayerServiceStub();
            stubService.Players.AddRange(new List<Player>
            {
                new Player
                {
                    Id = 1,
                    FirstName = "Aryna",
                    LastName = "Sabalenka",
                    Height = 1.82,
                    BirthDate = new DateTime(1998, 5, 5),
                    HandPlay = Entities.HandPlay.Right,
                    Nationality = "Belarus"
                },
                new Player
                {
                    Id = 2,
                    FirstName = "Iga",
                    LastName = "Swiatek",
                    Height = 1.76,
                    BirthDate = new DateTime(2001, 5, 31),
                    HandPlay = Entities.HandPlay.Right,
                    Nationality = "Poland"
                }
            });
            var controller = new PlayersController(stubService, _logger);
            var pagination = new PaginationDto { Index = 0, Count = 1 };

            // Act : appel avec pagination et criterium = 1
            var result = await controller.GetPlayers(pagination, 1);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            var response = okResult.Value as ApiResponseDto<List<PlayerDto>>;
            Assert.NotNull(response);
            Assert.Equal(2, response.Id); // id = 2 pour l'appel avec pagination
            Assert.Single(response.Result);
        }

        [Fact]
        public async Task PostPlayer_ReturnsCreatedAtActionResult_WithNewPlayer()
        {
            // Création d'un stub spécifique pour ce test (pas besoin de données pré-remplies)
            var stubService = new PlayerServiceStub();
            var controller = new PlayersController(stubService, _logger);
            var newPlayerDto = new PlayerDto
            {
                Id = 0, // l'ID est ignoré lors de la création
                FirstName = "Jelena",
                LastName = "Ostapenko",
                Height = 1.77,
                BirthDate = new DateTime(1997, 6, 8),
                HandPlay = Dto.HandPlay.Right,
                Nationality = "Latvia"
            };

            // Act
            var result = await controller.PostPlayer(newPlayerDto);
            var createdAtResult = result as CreatedAtActionResult;

            // Assert
            Assert.NotNull(createdAtResult);
            Assert.Equal(nameof(PlayersController.GetPlayerById), createdAtResult.ActionName);
            var returnedDto = createdAtResult.Value as PlayerDto;
            Assert.NotNull(returnedDto);
            Assert.Equal(100, returnedDto.Id); // le stub attribue l'ID 100
        }
    }
}
