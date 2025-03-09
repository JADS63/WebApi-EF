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
using Dto;
using WtaApi.Mappers;

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
        public async Task GetPlayerById_ExistingId_ReturnsOkResultWithPlayerDto()
        {
            // Arrange
            var mockService = new Mock<IPlayerService>();
            var expectedPlayer = new Player { Id = 1, FirstName = "Test", LastName = "Player", Nationality = "Test", BirthDate = DateTime.Now, Height = 1.8, HandPlay = Entities.HandPlay.Right };
            mockService.Setup(service => service.GetPlayerByIdAsync(1)).ReturnsAsync(expectedPlayer);
            var controller = new PlayersController(mockService.Object, NullLogger<PlayersController>.Instance);

            // Act
            var result = await controller.GetPlayerById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var playerDto = Assert.IsType<PlayerDto>(okResult.Value); 
            Assert.Equal(expectedPlayer.Id, playerDto.Id);
            Assert.Equal(expectedPlayer.FirstName, playerDto.FirstName);
            Assert.Equal(expectedPlayer.LastName, playerDto.LastName);
        }

        [Fact]
        public async Task GetPlayerById_NonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            var mockService = new Mock<IPlayerService>();
            mockService.Setup(service => service.GetPlayerByIdAsync(It.IsAny<int>())).ReturnsAsync((Player)null);
            var controller = new PlayersController(mockService.Object, NullLogger<PlayersController>.Instance);

            // Act
            var result = await controller.GetPlayerById(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task AddPlayer_ValidPlayer_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var mockService = new Mock<IPlayerService>();
            var newPlayerDto = new PlayerDto { FirstName = "New", LastName = "Player", Nationality = "Test", BirthDate = DateTime.Now, Height = 1.8, HandPlay = Entities.HandPlay.Right }; // Utilisez Entities.HandPlay
            var newPlayer = new Player { Id = 1, FirstName = "New", LastName = "Player", Nationality = "Test", BirthDate = DateTime.Now, Height = 1.8, HandPlay = Entities.HandPlay.Right };

            mockService.Setup(service => service.AddPlayerAsync(It.IsAny<Player>()))
                       .ReturnsAsync((Player p) => { p.Id = 1; return p; }); 

            var controller = new PlayersController(mockService.Object, NullLogger<PlayersController>.Instance);

            // Act
            var result = await controller.AddPlayer(newPlayerDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(PlayersController.GetPlayerById), createdAtActionResult.ActionName);
            Assert.Equal(1, createdAtActionResult.RouteValues["id"]);
            var returnedPlayerDto = Assert.IsType<PlayerDto>(createdAtActionResult.Value); 
            Assert.Equal("New", returnedPlayerDto.FirstName);


            mockService.Verify(service => service.AddPlayerAsync(It.IsAny<Player>()), Times.Once);
        }


        [Fact]
        public async Task UpdatePlayer_ValidPlayer_ReturnsOkResult()
        {
            // Arrange
            var mockService = new Mock<IPlayerService>();
            var existingPlayer = new Player { Id = 1, FirstName = "Old", LastName = "Name", Nationality = "Test", BirthDate = DateTime.Now, Height = 1.8, HandPlay = Entities.HandPlay.Right };
            var updatedPlayerDto = new PlayerDto { Id = 1, FirstName = "Updated", LastName = "Name", Nationality = "Test", BirthDate = DateTime.Now, Height = 1.8, HandPlay = Entities.HandPlay.Right }; // Utilisez Entities.HandPlay
            var updatedPlayer = new Player { Id = 1, FirstName = "Updated", LastName = "Name", Nationality = "Test", BirthDate = DateTime.Now, Height = 1.8, HandPlay = Entities.HandPlay.Right };

            mockService.Setup(service => service.UpdatePlayerAsync(It.IsAny<int>(), It.IsAny<Player>())).ReturnsAsync(updatedPlayer);
            var controller = new PlayersController(mockService.Object, NullLogger<PlayersController>.Instance);

            // Act
            var result = await controller.UpdatePlayer(1, updatedPlayerDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPlayerDto = Assert.IsType<PlayerDto>(okResult.Value); 
            Assert.Equal("Updated", returnedPlayerDto.FirstName);
            mockService.Verify(service => service.UpdatePlayerAsync(It.IsAny<int>(), It.IsAny<Player>()), Times.Once);
        }

        [Fact]
        public async Task UpdatePlayer_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var mockService = new Mock<IPlayerService>();
            var updatedPlayerDto = new PlayerDto { Id = 1, FirstName = "Updated", LastName = "Name" };
            var controller = new PlayersController(mockService.Object, NullLogger<PlayersController>.Instance);

            // Act
            var result = await controller.UpdatePlayer(2, updatedPlayerDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdatePlayer_PlayerNotFound_ReturnsNotFound()
        {
            // Arrange
            var mockService = new Mock<IPlayerService>();
            var updatedPlayerDto = new PlayerDto { Id = 1, FirstName = "Updated", LastName = "Name" };
            mockService.Setup(service => service.UpdatePlayerAsync(It.IsAny<int>(), It.IsAny<Player>())).ReturnsAsync((Player)null);
            var controller = new PlayersController(mockService.Object, NullLogger<PlayersController>.Instance);

            // Act
            var result = await controller.UpdatePlayer(1, updatedPlayerDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeletePlayer_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var mockService = new Mock<IPlayerService>();
            mockService.Setup(service => service.DeletePlayerAsync(1)).ReturnsAsync(true);
            var controller = new PlayersController(mockService.Object, NullLogger<PlayersController>.Instance);

            // Act
            var result = await controller.DeletePlayer(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeletePlayer_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var mockService = new Mock<IPlayerService>();
            mockService.Setup(service => service.DeletePlayerAsync(It.IsAny<int>())).ReturnsAsync(false);
            var controller = new PlayersController(mockService.Object, NullLogger<PlayersController>.Instance);

            // Act
            var result = await controller.DeletePlayer(99);

            // Assert
            Assert.IsType<NotFoundResult>(result); 
        }
    }
}