using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Services;
using Entities;
using WebApplicationJuaraujoda.Controllers;
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
            _controller = new PlayersController(_mockService.Object, NullLogger<PlayersController>.Instance);
        }

        [Fact]
        public async Task AddPlayer_ReturnsCreatedAtActionResult_WithCreatedPlayer()
        {
            // Arrange
            var newPlayer = new Player
            {
                // When creating a new player, the incoming ID is ignored.
                Id = 0,
                FirstName = "Jelena",
                LastName = "Ostapenko",
                Height = 1.77,
                BirthDate = new DateTime(1997, 6, 8),
                // Use Entities.HandPlay to avoid ambiguity
                HandPlay = Entities.HandPlay.Right,
                Nationality = "Latvia"
            };

            var createdPlayer = new Player
            {
                Id = 51,
                FirstName = newPlayer.FirstName,
                LastName = newPlayer.LastName,
                Height = newPlayer.Height,
                BirthDate = newPlayer.BirthDate,
                HandPlay = newPlayer.HandPlay,
                Nationality = newPlayer.Nationality
            };

            _mockService.Setup(s => s.AddPlayerAsync(It.IsAny<Player>())).ReturnsAsync(createdPlayer);

            // Act
            var result = await _controller.AddPlayer(newPlayer);
            var createdAtResult = result as CreatedAtActionResult;

            // Assert
            Assert.NotNull(createdAtResult);
            Assert.Equal(nameof(PlayersController.GetPlayerById), createdAtResult.ActionName);
            var returnedPlayer = createdAtResult.Value as Player;
            Assert.NotNull(returnedPlayer);
            Assert.Equal(51, returnedPlayer.Id);
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

            // Act
            var result = await _controller.UpdatePlayer(id, updatedPlayer);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            var returnedPlayer = okResult.Value as Player;
            Assert.NotNull(returnedPlayer);
            Assert.Equal(id, returnedPlayer.Id);
            Assert.Equal("Beatriz", returnedPlayer.FirstName);
            Assert.Equal("Hadda Maia", returnedPlayer.LastName);
        }
    }
}
