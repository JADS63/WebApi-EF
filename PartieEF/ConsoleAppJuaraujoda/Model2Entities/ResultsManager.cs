// Model2Entities/ResultsManager.cs
using Model;
using Shared;
using Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Numerics;
using System.Linq;

namespace Model2Entities
{
    public class ResultsManager : IResultsService<Player, Tournament>
    {
        private readonly IResultsService<PlayerEntity, TournamentEntity> _entityService;
        private readonly IPlayersService<Player> _playerService;
        private readonly ITournamentsService<Tournament> _tournamentService;

        public ResultsManager(IResultsService<PlayerEntity, TournamentEntity> entityService, IPlayersService<Player> playerService, ITournamentsService<Tournament> tournamentService)
        {
            _entityService = entityService;
            _playerService = playerService;
            _tournamentService = tournamentService;
        }

        public async Task<List<ResultEntity>> GetAllAsync(Expression<Func<ResultEntity, bool>>? predicate = null, string? includeProperties = null)
        {
            var entities = await _entityService.GetAllAsync(predicate, includeProperties);
            return entities;
        }

        public async Task<ResultEntity?> GetByIdAsync(int id)
        {
            var entity = await _entityService.GetByIdAsync(id);
            return entity;
        }

        public async Task<ResultEntity> AddAsync(ResultEntity result)
        {
            var entity = result;
            var addedEntity = await _entityService.AddAsync(entity);
            return addedEntity;
        }

        public async Task UpdateAsync(ResultEntity result)
        {
            var entity = result;
            await _entityService.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _entityService.DeleteAsync(id);
        }

        public async Task<List<ResultEntity>> GetResultsByPlayerIdAsync(int playerId)
        {
            return await _entityService.GetResultsByPlayerIdAsync(playerId);
        }

        public async Task<List<ResultEntity>> GetResultsByTournamentIdAsync(int tournamentId)
        {
            var resultEntities = await _entityService.GetResultsByTournamentIdAsync(tournamentId);
            return resultEntities;
        }

        public async Task<List<ResultEntity>> GetResultsByPlayerAsync(Player player)
        {
            var entityResults = await _entityService.GetResultsByPlayerAsync(player.ToEntity());
            return entityResults;
        }

        public async Task<List<ResultEntity>> GetResultsByTournamentAsync(Tournament tournament)
        {
            var entityResults = await _entityService.GetResultsByTournamentAsync(tournament.ToEntity());
            return entityResults;
        }

        public async Task AddResultToPlayerAsync(int playerId, int tournamentId, ResultEntity result)
        {
            await _entityService.AddResultToPlayerAsync(playerId, tournamentId, result);
        }

        public async Task RemoveResultFromPlayerAsync(int playerId, int tournamentId, int resultId)
        {
            await _entityService.RemoveResultFromPlayerAsync(playerId, tournamentId, resultId);
        }

        // NOUVELLES MÉTHODES POUR LA PARTIE 3B -
        public async Task<List<ResultEntity>> GetFullResultsByTournamentIdAsync(int tournamentId)
        {
            var resultEntities = await _entityService.GetFullResultsByTournamentIdAsync(tournamentId);
            return resultEntities;
        }

        public async Task<List<ResultEntity>> GetFullResultsByTournamentAsync(Tournament tournament)
        {
            return await GetFullResultsByTournamentIdAsync(tournament.Id);
        }


        public void Dispose()
        {
            _entityService.Dispose();
        }
    }
}