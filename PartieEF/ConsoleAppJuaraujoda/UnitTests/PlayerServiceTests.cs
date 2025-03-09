using ContextLib;
using Entities;
using Microsoft.EntityFrameworkCore;
using StubbedContextLib;
using Xunit;
using System.Linq;

namespace UnitTests
{
    public class PlayerServiceTests : IDisposable
    {
        private readonly StubbedContext _context;
        private readonly DbPlayersManager _playerService;

        public PlayerServiceTests()
        {
            var options = new DbContextOptionsBuilder<WtaContext>()
                .UseInMemoryDatabase(databaseName: "TestPlayersDb")
                .Options;
            _context = new StubbedContext(options);
            _context.Database.EnsureCreated(); 
            _playerService = new DbPlayersManager(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted(); 
            _context.Dispose();
            _playerService.Dispose();
        }

        [Fact]
        public async Task AddPlayerAsync_AddsPlayerToDatabase()
        {
            // Arrange
            var newPlayer = new PlayerEntity { Id = 100, FirstName = "Test", LastName = "Player", BirthDate = new DateTime(2000, 1, 1), Height = 1.70f, Nationality = "TestNation", Handplay = HandplayEntity.Right };

            // Act
            await _playerService.AddAsync(newPlayer);
            var playerFromDb = await _playerService.GetByIdAsync(100);

            // Assert
            Assert.NotNull(playerFromDb);
            Assert.Equal("Test", playerFromDb.FirstName);
        }

        [Fact]
        public async Task DeletePlayerAsync_RemovesPlayerFromDatabase()
        {
            // Arrange
            var playerToDelete = new PlayerEntity { Id = 101, FirstName = "ToDelete", LastName = "Player", BirthDate = new DateTime(2000, 1, 1), Height = 1.70f, Nationality = "TestNation", Handplay = HandplayEntity.Right };
            _context.Players.Add(playerToDelete);
            await _context.SaveChangesAsync();

            // Act
            await _playerService.DeleteAsync(101);
            var deletedPlayer = await _playerService.GetByIdAsync(101);

            // Assert
            Assert.Null(deletedPlayer);
        }

        [Fact]
        public async Task UpdatePlayerAsync_ModifiesPlayerInDatabase()
        {
            // Arrange
            var playerToUpdate = new PlayerEntity { Id = 102, FirstName = "Original", LastName = "Name", BirthDate = new DateTime(2000, 1, 1), Height = 1.70f, Nationality = "TestNation", Handplay = HandplayEntity.Right };
            _context.Players.Add(playerToUpdate);
            await _context.SaveChangesAsync();

            // Act
            playerToUpdate.FirstName = "Updated";
            await _playerService.UpdateAsync(playerToUpdate);
            var updatedPlayer = await _playerService.GetByIdAsync(102);

            // Assert
            Assert.NotNull(updatedPlayer);
            Assert.Equal("Updated", updatedPlayer.FirstName);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectPlayer()
        {
            // Arrange
            var playerToGet = new PlayerEntity { Id = 103, FirstName = "Get", LastName = "Player", BirthDate = new DateTime(2000, 1, 1), Height = 1.70f, Nationality = "TestNation", Handplay = HandplayEntity.Right };
            _context.Players.Add(playerToGet);
            await _context.SaveChangesAsync();

            // Act
            var retrievedPlayer = await _playerService.GetByIdAsync(103);

            // Assert
            Assert.NotNull(retrievedPlayer);
            Assert.Equal(103, retrievedPlayer.Id);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllPlayers()
        {
            // Arrange
            _context.Players.AddRange(
                new PlayerEntity { Id = 104, FirstName = "Player1", LastName = "Test", BirthDate = new DateTime(2000, 1, 1), Height = 1.70f, Nationality = "TestNation", Handplay = HandplayEntity.Right },
                new PlayerEntity { Id = 105, FirstName = "Player2", LastName = "Test", BirthDate = new DateTime(2001, 1, 1), Height = 1.70f, Nationality = "TestNation", Handplay = HandplayEntity.Right }
            );
            await _context.SaveChangesAsync();

            // Act
            var allPlayers = await _playerService.GetAllAsync();

            // Assert
            Assert.NotNull(allPlayers);
            Assert.True(allPlayers.Count >= 2); 
            Assert.Contains(allPlayers, p => p.FirstName == "Player1");
            Assert.Contains(allPlayers, p => p.FirstName == "Player2");
        }
    }
}