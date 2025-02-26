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
        public void PostPlayer_ReturnsCreatedAtActionResult_WithCreatedPlayerDto()
        {
            // Arrange
            var newPlayerDto = new PlayerDto
            {
                Id = 0, // l'ID est ignoré lors de la création
                FirstName = "Jelena",
                LastName = "Ostapenko",
                Height = 1.77,
                BirthDate = new DateTime(1997, 6, 8),
                // Utilisation directe de l'énumération Dto.HandPlay.Right
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

            _mockService.Setup(s => s.AddPlayer(It.IsAny<Player>())).Returns(createdPlayer);

            // Act
            var result = _controller.PostPlayer(newPlayerDto) as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(nameof(PlayersController.GetPlayerById), result.ActionName);
            var returnedDto = result.Value as PlayerDto;
            Assert.NotNull(returnedDto);
            Assert.Equal(51, returnedDto.Id);
        }

        [Fact]
        public void PutPlayer_ReturnsOkResult_WithUpdatedPlayerDto()
        {
            // Arrange
            int id = 51;
            var updatedPlayerDto = new PlayerDto
            {
                Id = 0, // l'ID est ignoré dans le DTO lors de l'update
                FirstName = "Beatriz",
                LastName = "Hadda Maia",
                Height = 1.85,
                BirthDate = new DateTime(1996, 5, 30),
                // Utilisation directe de l'énumération Dto.HandPlay.Left
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

            _mockService.Setup(s => s.UpdatePlayer(id, It.IsAny<Player>())).Returns(updatedPlayer);

            // Act
            var result = _controller.PutPlayer(id, updatedPlayerDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var returnedDto = result.Value as PlayerDto;
            Assert.NotNull(returnedDto);
            Assert.Equal(id, returnedDto.Id);
            Assert.Equal("Beatriz", returnedDto.FirstName);
            Assert.Equal("Hadda Maia", returnedDto.LastName);
        }
    }
}
