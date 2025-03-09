using Xunit;
using Microsoft.AspNetCore.Mvc;
using Entities;
using Microsoft.Extensions.Logging.Abstractions;
using Services;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using WebApplicationJuaraujoda.Controllers;
using Dto;
using WtaApi.Mappers;

namespace WebApplicationJuaraujoda.Tests
{
    public class PlayersControllerTests
    {
        [Fact]
        public async Task GetPlayers_WithPagination_ReturnsPaginatedResult()
        {
            var stubService = new PlayerServiceStub();
            stubService.Players.AddRange(new List<Player>
    {
        new Player { Id = 1, FirstName = "Aryna", LastName = "Sabalenka", Height = 1.82, BirthDate = new DateTime(1998, 5, 5), HandPlay = Entities.HandPlay.Right, Nationality = "Belarus" },
        new Player { Id = 2, FirstName = "Iga", LastName = "Swiatek", Height = 1.76, BirthDate = new DateTime(2001, 5, 31), HandPlay = Entities.HandPlay.Right, Nationality = "Poland" }
    });
            var controller = new PlayersController(stubService, NullLogger<PlayersController>.Instance);

            int index = 0;
            int count = 1;
            Entities.SortCriteria sort = Entities.SortCriteria.ByNameThenFirstName;

            // Act
            var result = await controller.GetPlayers(index, count, sort);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var paginatedResponseDto = Assert.IsType<PaginatedResponseDto<PlayerDto>>(okResult.Value);
            Assert.NotNull(paginatedResponseDto);
            Assert.NotNull(paginatedResponseDto.Items);
            Assert.Single(paginatedResponseDto.Items);
            Assert.Equal(10, paginatedResponseDto.TotalCount); 
            Assert.Equal(0, paginatedResponseDto.PageIndex);
            Assert.Equal(1, paginatedResponseDto.CountPerPage);
        }

        [Fact]
        public async Task GetPlayerById_ExistingId_ReturnsOkResultWithPlayerDto()
        {
            // Arrange
            var stubService = new PlayerServiceStub();
            var expectedPlayer = new Player { Id = 42, FirstName = "Aryna", LastName = "Sabalenka", Height = 1.82, BirthDate = new DateTime(1998, 5, 5), HandPlay = Entities.HandPlay.Right, Nationality = "Belarus" };
            stubService.Players.Add(expectedPlayer);
            var controller = new PlayersController(stubService, NullLogger<PlayersController>.Instance);

            // Act
            var result = await controller.GetPlayerById(42);

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
            var stubService = new PlayerServiceStub();
            var controller = new PlayersController(stubService, NullLogger<PlayersController>.Instance);

            // Act
            var result = await controller.GetPlayerById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}