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

namespace WebApplicationJuaraujoda.Tests
{
    public class PlayersControllerTests
    {
        [Fact]
        public async Task GetPlayers_WithPagination_ReturnsPaginatedResult()
        {
            // Arrange: Create a stub service instance with preset data
            var stubService = new Services.PlayerServiceStub();
            // Ensure the stub exposes its Players property (make it public in your stub)
            stubService.Players.AddRange(new List<Player>
            {
                new Player { Id = 1, FirstName = "Aryna", LastName = "Sabalenka", Height = 1.82, BirthDate = new DateTime(1998, 5, 5), HandPlay = Entities.HandPlay.Right, Nationality = "Belarus" },
                new Player { Id = 2, FirstName = "Iga", LastName = "Swiatek", Height = 1.76, BirthDate = new DateTime(2001, 5, 31), HandPlay = Entities.HandPlay.Right, Nationality = "Poland" }
            });
            var controller = new PlayersController(stubService, NullLogger<PlayersController>.Instance);

            // Use integer values for pagination parameters: index, count, sort criteria
            int index = 0, count = 1, sort = 0;

            // Act: call the GetPlayers action
            var result = await controller.GetPlayers(index, count, sort);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            dynamic value = okResult.Value;
            IEnumerable<Player> players = value.result as IEnumerable<Player>;
            Assert.NotNull(players);
            Assert.Single(players);
        }
    }
}
