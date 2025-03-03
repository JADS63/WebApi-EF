using Xunit;
using Moq;
using WebApplicationJuaraujoda.Controllers;
using Microsoft.AspNetCore.Mvc;
using Entities;
using WtaApi.Mappers;
using Dto;
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
        public async Task PostPlayer_ReturnsCreatedAtActionResult_WithCreatedPlayerDto()
        {
            // Arrange
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

            var createdPlayer = new Player
            {
                Id = 51,
                FirstName = newPlayerDto.FirstName,
                LastName = newPlayerDto.LastName,
                Height = newPlayerDto.Height,
                BirthDate = newPlayerDto.BirthDate,
                HandPlay = Entities.HandPlay.Right,
                Nationality = newPlayerDto.Nationality
            };

            _mockService.Setup(s => s.AddPlayerAsync(It.IsAny<Player>())).ReturnsAsync(createdPlayer);

            // Act
            var result = await _controller.PostPlayer(newPlayerDto);
            var createdAtResult = result as CreatedAtActionResult;

            // Assert
            Assert.NotNull(createdAtResult);
            Assert.Equal(nameof(PlayersController.GetPlayerById), createdAtResult.ActionName);
            var returnedDto = createdAtResult.Value as PlayerDto;
            Assert.NotNull(returnedDto);
            Assert.Equal(51, returnedDto.Id);
        }

        [Fact]
        public async Task PutPlayer_ReturnsOkResult_WithUpdatedPlayerDto()
        {
            // Arrange
            int id = 51;
            var updatedPlayerDto = new PlayerDto
            {
                Id = 0, // l'ID est ignoré dans le DTO lors de la mise à jour
                FirstName = "Beatriz",
                LastName = "Hadda Maia",
                Height = 1.85,
                BirthDate = new DateTime(1996, 5, 30),
                HandPlay = Dto.HandPlay.Left,
                Nationality = "Brazil"
            };

            var updatedPlayer = new Player
            {
                Id = id,
                FirstName = updatedPlayerDto.FirstName,
                LastName = updatedPlayerDto.LastName,
                Height = updatedPlayerDto.Height,
                BirthDate = updatedPlayerDto.BirthDate,
                HandPlay = Entities.HandPlay.Left,
                Nationality = updatedPlayerDto.Nationality
            };

            _mockService.Setup(s => s.UpdatePlayerAsync(id, It.IsAny<Player>())).ReturnsAsync(updatedPlayer);

            // Act
            var result = await _controller.PutPlayer(id, updatedPlayerDto);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            var returnedDto = okResult.Value as PlayerDto;
            Assert.NotNull(returnedDto);
            Assert.Equal(id, returnedDto.Id);
            Assert.Equal("Beatriz", returnedDto.FirstName);
            Assert.Equal("Hadda Maia", returnedDto.LastName);
        }
    }
}
