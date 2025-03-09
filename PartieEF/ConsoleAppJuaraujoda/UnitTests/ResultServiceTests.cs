using ContextLib;
using Entities;
using Microsoft.EntityFrameworkCore;
using StubbedContextLib;
using Xunit; 
using System.Linq;

namespace UnitTests
{
    public class ResultServiceTests : IDisposable
    {
        private readonly StubbedContext _context;
        private readonly DbResultsManager _resultService;
        private readonly DbPlayersManager _playerService;
        private readonly DbTournamentsManager _tournamentService; 

        public ResultServiceTests()
        {
            var options = new DbContextOptionsBuilder<WtaContext>()
                .UseInMemoryDatabase(databaseName: "TestResultsDb")
                .Options;
            _context = new StubbedContext(options);
            _context.Database.EnsureCreated();
            _playerService = new DbPlayersManager(_context);
            _tournamentService = new DbTournamentsManager(_context);
            _resultService = new DbResultsManager(_context, _playerService, _tournamentService);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
            _resultService.Dispose();
            _playerService.Dispose();
            _tournamentService.Dispose();
        }

        [Fact]
        public async Task AddResultAsync_AddsResultToDatabase()
        {
            // Arrange
            var newResult = new ResultEntity { Id = 200, Result = Result.Quarterfinal };

            // Act
            var addedResult = await _resultService.AddAsync(newResult);
            var resultFromDb = await _resultService.GetByIdAsync(addedResult.Id);

            // Assert
            Assert.NotNull(resultFromDb);
            Assert.Equal(Result.Quarterfinal, resultFromDb.Result);
        }

        [Fact]
        public async Task AddResultToPlayerAsync_AssociatesResultWithPlayerAndTournament()
        {
            // Arrange
            var player = new PlayerEntity { Id = 102, FirstName = "Test", LastName = "Player2", BirthDate = new DateTime(2000, 1, 1), Height = 1.70f, Nationality = "TestNation", Handplay = HandplayEntity.Right };
            var tournament = new TournamentEntity { Id = 102, Name = "Test Tournament", Year = 2025 };
            _context.Players.Add(player);
            _context.Tournaments.Add(tournament);
            await _context.SaveChangesAsync();

            var newResult = new ResultEntity { Id = 201, Result = Result.Win };

            // Act
            await _resultService.AddResultToPlayerAsync(102, 102, newResult);
            var resultFromDb = await _resultService.GetByIdAsync(201);

            // Assert
            Assert.NotNull(resultFromDb);
            Assert.Contains(player, resultFromDb.Players);
            Assert.Contains(tournament, resultFromDb.Tournaments);
        }

        [Fact]
        public async Task UpdateResultAsync_ModifiesResultInDatabase()
        {
            // Arrange
            var resultToUpdate = new ResultEntity { Id = 202, Result = Result.Quarterfinal };
            _context.Results.Add(resultToUpdate);
            await _context.SaveChangesAsync();

            // Act
            resultToUpdate.Result = Result.Final;
            await _resultService.UpdateAsync(resultToUpdate);
            var updatedResult = await _resultService.GetByIdAsync(202);

            // Assert
            Assert.NotNull(updatedResult);
            Assert.Equal(Result.Final, updatedResult.Result);
        }

        [Fact]
        public async Task DeleteResultAsync_RemovesResultFromDatabase()
        {
            // Arrange
            var resultToDelete = new ResultEntity { Id = 203, Result = Result.Quarterfinal };
            _context.Results.Add(resultToDelete);
            await _context.SaveChangesAsync();

            // Act
            await _resultService.DeleteAsync(203);
            var deletedResult = await _resultService.GetByIdAsync(203);

            // Assert
            Assert.Null(deletedResult);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectResult()
        {
            // Arrange
            var resultToGet = new ResultEntity { Id = 204, Result = Result.Quarterfinal };
            _context.Results.Add(resultToGet);
            await _context.SaveChangesAsync();

            // Act
            var retrievedResult = await _resultService.GetByIdAsync(204);

            // Assert
            Assert.NotNull(retrievedResult);
            Assert.Equal(204, retrievedResult.Id);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllResults()
        {
            // Arrange
            _context.Results.AddRange(
                new ResultEntity { Id = 205, Result = Result.Quarterfinal },
                new ResultEntity { Id = 206, Result = Result.Semifinal }
            );
            await _context.SaveChangesAsync();

            // Act
            var allResults = await _resultService.GetAllAsync();

            // Assert
            Assert.NotNull(allResults);
            Assert.True(allResults.Count >= 2); 
            Assert.Contains(allResults, r => r.Result == Result.Quarterfinal);
            Assert.Contains(allResults, r => r.Result == Result.Semifinal);
        }

        [Fact]
        public async Task GetResultsByPlayerIdAsync_ReturnsResultsForPlayer()
        {
            // Arrange
            var player = new PlayerEntity { Id = 103, FirstName = "Test", LastName = "Player3", BirthDate = new DateTime(2000, 1, 1), Height = 1.70f, Nationality = "TestNation", Handplay = HandplayEntity.Right };
            var result1 = new ResultEntity { Id = 207, Result = Result.Round16 };
            var result2 = new ResultEntity { Id = 208, Result = Result.Round32 };
            player.Results.Add(result1);
            player.Results.Add(result2);
            _context.Players.Add(player);
            _context.Results.AddRange(result1, result2);
            await _context.SaveChangesAsync();

            // Act
            var playerResults = await _resultService.GetResultsByPlayerIdAsync(103);

            // Assert
            Assert.NotNull(playerResults);
            Assert.True(playerResults.Count == 2);
            Assert.Contains(playerResults, r => r.Id == 207);
            Assert.Contains(playerResults, r => r.Id == 208);
        }

        [Fact]
        public async Task GetResultsByTournamentIdAsync_ReturnsResultsForTournament()
        {
            // Arrange
            var tournament = new TournamentEntity { Id = 103, Name = "Test Tournament2", Year = 2026 };
            var result1 = new ResultEntity { Id = 209, Result = Result.Round64 };
            var result2 = new ResultEntity { Id = 210, Result = Result.Round128 };
            tournament.Results.Add(result1);
            tournament.Results.Add(result2);
            _context.Tournaments.Add(tournament);
            _context.Results.AddRange(result1, result2);
            await _context.SaveChangesAsync();

            // Act
            var tournamentResults = await _resultService.GetResultsByTournamentIdAsync(103);

            // Assert
            Assert.NotNull(tournamentResults);
            Assert.True(tournamentResults.Count == 2);
            Assert.Contains(tournamentResults, r => r.Id == 209);
            Assert.Contains(tournamentResults, r => r.Id == 210);
        }

        [Fact]
        public async Task RemoveResultFromPlayerAsync_DissociatesResultFromPlayerAndTournament()
        {
            // Arrange
            var player = new PlayerEntity { Id = 104, FirstName = "Test", LastName = "Player4", BirthDate = new DateTime(2000, 1, 1), Height = 1.70f, Nationality = "TestNation", Handplay = HandplayEntity.Right };
            var tournament = new TournamentEntity { Id = 104, Name = "Test Tournament3", Year = 2027 };
            var resultToRemove = new ResultEntity { Id = 211, Result = Result.NotPlayed };
            resultToRemove.Players.Add(player);
            resultToRemove.Tournaments.Add(tournament);
            _context.Players.Add(player);
            _context.Tournaments.Add(tournament);
            _context.Results.Add(resultToRemove);
            await _context.SaveChangesAsync();

            // Act
            await _resultService.RemoveResultFromPlayerAsync(104, 104, 211);
            var resultFromDb = await _resultService.GetByIdAsync(211); 

            // Assert
            Assert.NotNull(resultFromDb); 
            Assert.Empty(resultFromDb.Players); 
            Assert.Empty(resultFromDb.Tournaments); 
        }
    }
}