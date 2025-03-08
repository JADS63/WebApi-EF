using Microsoft.AspNetCore.Mvc;
using Entities;
using Microsoft.Extensions.Logging.Abstractions;
using Services;
using WebApplicationJuaraujoda.Controllers;

namespace WebApplicationJuaraujoda.Tests
{
    public class PlayersControllerTests
    {
        [Fact]
        public async Task GetPlayers_WithPagination_ReturnsPaginatedResult()
        {
            // Arrange : création d'un stub de service avec des données
            var stubService = new PlayerServiceStub();
            stubService.Players.AddRange(new List<Player>
            {
                new Player { Id = 1, FirstName = "Aryna", LastName = "Sabalenka", Height = 1.82, BirthDate = new System.DateTime(1998, 5, 5), HandPlay = Entities.HandPlay.Right, Nationality = "Belarus" },
                new Player { Id = 2, FirstName = "Iga", LastName = "Swiatek", Height = 1.76, BirthDate = new System.DateTime(2001, 5, 31), HandPlay = Entities.HandPlay.Right, Nationality = "Poland" }
            });
            var controller = new PlayersController(stubService, NullLogger<PlayersController>.Instance);

            // Définir les paramètres de pagination
            int index = 0, count = 1, sort = 0;

            // Act : appel de l'action GetPlayers avec pagination
            var result = await controller.GetPlayers(index, count, sort);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            dynamic value = okResult.Value;
            IEnumerable<Player> players = value.result as IEnumerable<Player>;
            Assert.NotNull(players);
            Assert.Single(players);
        }
    }
}
