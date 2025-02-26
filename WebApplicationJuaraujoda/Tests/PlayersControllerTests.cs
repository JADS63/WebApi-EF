using Xunit;
using Moq;
using WebApplicationJuaraujoda.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Entities;
using WtaApi.Mappers;
using Dto;
using System.Linq;
using System;
using Microsoft.Extensions.Logging.Abstractions;
using Services;

namespace WebApplicationJuaraujoda.Tests
{
    public class PlayersControllerTests
    {
        private readonly PlayersController _controller;
        private readonly Mock<IPlayerService> _mockService;

        public PlayersControllerTests()
        {
            _mockService = new Mock<IPlayerService>();
            // Fournir un logger nul pour satisfaire le paramètre obligatoire
            _controller = new PlayersController(_mockService.Object, NullLogger<PlayersController>.Instance);
        }

        [Fact]
        public void GetAll_ReturnsOkResult_WithPlayers()
        {
            // Arrange
            var players = new List<Player>
            {
                new Player
                {
                    Id = 42,
                    FirstName = "Aryna",
                    LastName = "Sabalenka",
                    Height = 1.82,
                    BirthDate = new DateTime(1998, 5, 5),
                    HandPlay = Entities.HandPlay.Right,
                    Nationality = "Belarus"
                },
                new Player
                {
                    Id = 43,
                    FirstName = "Iga",
                    LastName = "Swiatek",
                    Height = 1.76,
                    BirthDate = new DateTime(2001, 5, 31),
                    HandPlay = Entities.HandPlay.Right,
                    Nationality = "Poland"
                }
            };

            _mockService.Setup(s => s.GetPlayers()).Returns(players);

            // Act : appel sans paramètres => retourne toutes les joueuses avec id = 1
            var result = _controller.GetPlayers() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var response = result.Value as ApiResponse<List<PlayerDto>>;
            Assert.NotNull(response);
            Assert.Equal(1, response.id);
            Assert.NotNull(response.result);
            Assert.Equal(2, response.result.Count);
        }

        [Fact]
        public void GetPlayers_WithPagination_ReturnsOkResult_WithPaginatedPlayers()
        {
            // Arrange
            int index = 0;
            int count = 1;
            // Simuler le retour d'une seule joueuse paginée
            var players = new List<Player>
            {
                new Player
                {
                    Id = 42,
                    FirstName = "Aryna",
                    LastName = "Sabalenka",
                    Height = 1.82,
                    BirthDate = new DateTime(1998, 5, 5),
                    HandPlay = Entities.HandPlay.Right,
                    Nationality = "Belarus"
                }
            };

            // Nous définissons sortCriteria = 1 (par exemple, correspondant à Dto.SortCriteria.ByNameThenFirstName)
            _mockService.Setup(s => s.GetPlayers(index, count, 1)).Returns(players);

            var pagination = new PaginationDto { Index = index, Count = count };

            // Act : appel avec pagination et sortCriteria = 1 => retourne les joueuses paginées avec id = 2
            var result = _controller.GetPlayers(pagination, 1) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var response = result.Value as ApiResponse<List<PlayerDto>>;
            Assert.NotNull(response);
            Assert.Equal(2, response.id);
            Assert.NotNull(response.result);
            Assert.Single(response.result);
        }
    }
}
