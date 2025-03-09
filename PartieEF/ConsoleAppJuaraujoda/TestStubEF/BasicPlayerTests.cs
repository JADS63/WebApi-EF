using ContextLib;
using Entities;
using Microsoft.EntityFrameworkCore;
using Xunit; 

namespace TestStubEF
{
    public class BasicPlayerTests
    {
        [Fact]
        public void CanAddPlayerToContext()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<WtaContext>()
                .UseInMemoryDatabase(databaseName: "BasicTestDb") 
                .Options;

            using (var context = new WtaContext(options))
            {
                context.Database.EnsureCreated();

                var newPlayer = new PlayerEntity { Id = 1, FirstName = "Basic", LastName = "TestPlayer", BirthDate = DateTime.Now, Height = 1.70f, Nationality = "Test", Handplay = HandplayEntity.Right };

                // Act
                context.Players.Add(newPlayer);
                context.SaveChanges();

                // Assert
                var playerFromDb = context.Players.Find(1);
                Assert.NotNull(playerFromDb);
                Assert.Equal("Basic", playerFromDb.FirstName); 
            }
        }
    }
}