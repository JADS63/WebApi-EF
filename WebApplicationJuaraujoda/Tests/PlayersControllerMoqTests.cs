using Xunit;
using Moq;
using WebApplicationJuaraujoda.Controllers;
using Microsoft.AspNetCore.Mvc;
using Entities;
using System;
using Microsoft.Extensions.Logging.Abstractions;
using Services;
using System.Threading.Tasks;

namespace WebApplicationJuaraujoda.Tests
{
    public class PlayersControllerMoqTests
    {
        private readonly PlayersController _controller;
        private readonly Mock<IPlayerService> _mockService;

        public PlayersControllerMoqTests()
        {
            _mockService = new Mock<IPlayerService>();
            // Utilisation de NullLogger pour satisfaire le paramètre logger
            _controller = new PlayersController(_mockService.Object, NullLogger<PlayersController>.Instance);
        }

        [Fact]
        public async Task AddPlayer_ReturnsCreatedAtActionResult_WithCreatedPlayer()
        {
            // Arrange
            // Création d'une entité Player pour simuler l'ajout (le contrôleur attend une Player, pas un DTO)
            var newPlayer = new Player
            {
                // L'ID est ignoré lors de la création
                FirstName = "Jelena",
                LastName = "Ostapenko",
                Height = 1.77,
                BirthDate = new DateTime(1997, 6, 8),
                // Ici, nous utilisons Entities.HandPlay pour lever l'ambiguïté
                HandPlay = Entities.HandPlay.Right,
                Nationality = "Latvia"
            };

            var createdPlayer = new Player
            {
                Id = 100,
                FirstName = newPlayer.FirstName,
                LastName = newPlayer.LastName,
                Height = newPlayer.Height,
                BirthDate = newPlayer.BirthDate,
                HandPlay = newPlayer.HandPlay,
                Nationality = newPlayer.Nationality
            };

            _mockService.Setup(s => s.AddPlayerAsync(It.IsAny<Player>())).ReturnsAsync(createdPlayer);

            // Act : appel de l'action AddPlayer du contrôleur
            var result = await _controller.AddPlayer(newPlayer);
            var createdAtResult = result as CreatedAtActionResult;

            // Assert
            Assert.NotNull(createdAtResult);
            Assert.Equal(nameof(PlayersController.GetPlayerById), createdAtResult.ActionName);
            // Extraction de l'objet retourné dans "result"
            dynamic value = createdAtResult.Value;
            Player returnedPlayer = value.result as Player;
            Assert.NotNull(returnedPlayer);
            Assert.Equal(100, returnedPlayer.Id);
        }

        [Fact]
        public async Task UpdatePlayer_ReturnsOkResult_WithUpdatedPlayer()
        {
            // Arrange
            int id = 51;
            var updatedPlayer = new Player
            {
                Id = id,
                FirstName = "Beatriz",
                LastName = "Hadda Maia",
                Height = 1.85,
                BirthDate = new DateTime(1996, 5, 30),
                HandPlay = Entities.HandPlay.Left,
                Nationality = "Brazil"
            };

            _mockService.Setup(s => s.UpdatePlayerAsync(id, It.IsAny<Player>())).ReturnsAsync(updatedPlayer);

            // Act : appel de l'action UpdatePlayer du contrôleur
            var result = await _controller.UpdatePlayer(id, updatedPlayer);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            dynamic value = okResult.Value;
            Player returnedPlayer = value.result as Player;
            Assert.NotNull(returnedPlayer);
            Assert.Equal(id, returnedPlayer.Id);
            Assert.Equal("Beatriz", returnedPlayer.FirstName);
            Assert.Equal("Hadda Maia", returnedPlayer.LastName);
        }
    }
}
