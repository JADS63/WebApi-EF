using Xunit;
using Moq;
using Services;
using WebApplicationJuaraujoda.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Entities;
using WtaApi.Mappers;
using Dto;
using System.Linq;
using System;

namespace WebApplicationJuaraujoda.Tests
{
    public class PlayersControllerTests
    {
        private readonly PlayersController _controller;
        private readonly Mock<IPlayerService> _mockService;

        public PlayersControllerTests()
        {
            _mockService = new Mock<IPlayerService>();
            _controller = new PlayersController(_mockService.Object);
        }

        [Fact]
        public void GetAll_ReturnsOkResult_WithPlayers()
        {
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

            var result = _controller.GetAll() as OkObjectResult;

            Assert.NotNull(result);
            var response = result.Value as ApiResponse<List<PlayerDto>>;
            Assert.Equal(1, response.id);
            Assert.NotNull(response.result);
            Assert.Equal(2, response.result.Count);
        }

        [Fact]
        public void GetPlayers_WithPagination_ReturnsOkResult_WithPaginatedPlayers()
        {
            int index = 0;
            int count = 1;
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

            _mockService.Setup(s => s.GetPlayers(index, count)).Returns(players);
            var pagination = new PaginationDto { Index = index, Count = count };

            var result = _controller.GetPlayers(pagination) as OkObjectResult;

            Assert.NotNull(result);
            var response = result.Value as ApiResponse<List<PlayerDto>>;
            Assert.Equal(2, response.id);
            Assert.NotNull(response.result);
            Assert.Single(response.result);
        }
    }
}
