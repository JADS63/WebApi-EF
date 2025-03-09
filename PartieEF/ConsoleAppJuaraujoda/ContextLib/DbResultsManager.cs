// ContextLib/DbResultsManager.cs
using Entities;
using Microsoft.EntityFrameworkCore;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ContextLib
{
    public class DbResultsManager : IResultsService<PlayerEntity, TournamentEntity>
    {
        private WtaContext Context { get; set; }
        private readonly IPlayersService<PlayerEntity> _playersService;
        private readonly ITournamentsService<TournamentEntity> _tournamentsService;

        public DbResultsManager(WtaContext context, IPlayersService<PlayerEntity> playersService, ITournamentsService<TournamentEntity> tournamentsService)
        {
            Context = context;
            _playersService = playersService;
            _tournamentsService = tournamentsService;
        }

        public async Task<List<ResultEntity>> GetAllAsync(Expression<Func<ResultEntity, bool>>? predicate = null, string? includeProperties = null)
        {
            IQueryable<ResultEntity> query = Context.Results;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<ResultEntity?> GetByIdAsync(int id)
        {
            return await Context.Results.FindAsync(id);
        }

        public async Task<ResultEntity> AddAsync(ResultEntity result)
        {
            Context.Results.Add(result);
            await Context.SaveChangesAsync();
            return result;
        }

        public async Task UpdateAsync(ResultEntity result)
        {
            Context.Entry(result).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var result = await Context.Results.FindAsync(id);
            if (result != null)
            {
                Context.Results.Remove(result);
                await Context.SaveChangesAsync();
            }
        }

        public async Task<List<ResultEntity>> GetResultsByPlayerIdAsync(int playerId)
        {
            return await Context.Results.Where(r => r.Players.Any(p => p.Id == playerId)).ToListAsync();
        }

        // MODIFICATION : Charger les relations (Players et Tournaments)
        public async Task<List<ResultEntity>> GetResultsByTournamentIdAsync(int tournamentId)
        {
            return await Context.Results
                .Where(r => r.Tournaments.Any(t => t.Id == tournamentId))
                .Include(r => r.Players) // Charger les joueurs associés
                .Include(r => r.Tournaments) // Charger les tournois associés
                .ToListAsync();
        }
        public async Task<List<ResultEntity>> GetResultsByPlayerAsync(PlayerEntity player)
        {
            return await Context.Results.Where(r => r.Players.Any(p => p.Id == player.Id)).ToListAsync();
        }
        public async Task<List<ResultEntity>> GetResultsByTournamentAsync(TournamentEntity tournament)
        {
            return await Context.Results.Where(r => r.Tournaments.Any(t => t.Id == tournament.Id)).ToListAsync();
        }

        public async Task AddResultToPlayerAsync(int playerId, int tournamentId, ResultEntity result)
        {
            var player = await _playersService.GetByIdAsync(playerId);
            var tournament = await _tournamentsService.GetByIdAsync(tournamentId);
            if (player != null && tournament != null)
            {
                result.Players.Add(player);
                result.Tournaments.Add(tournament);
                Context.Results.Add(result);
                await Context.SaveChangesAsync();
            }
        }

        public async Task RemoveResultFromPlayerAsync(int playerId, int tournamentId, int resultId)
        {
            var result = await Context.Results
                .Include(r => r.Players)
                .Include(r => r.Tournaments)
                .FirstOrDefaultAsync(r => r.Id == resultId && r.Players.Any(p => p.Id == playerId) && r.Tournaments.Any(t => t.Id == tournamentId));

            if (result != null)
            {
                var player = await _playersService.GetByIdAsync(playerId);
                var tournament = await _tournamentsService.GetByIdAsync(tournamentId);
                if (player != null) result.Players.Remove(player);
                if (tournament != null) result.Tournaments.Remove(tournament);

                await Context.SaveChangesAsync();
            }
        }

        // NOUVELLES MÉTHODES POUR LA PARTIE 3B
        public async Task<List<ResultEntity>> GetFullResultsByTournamentIdAsync(int tournamentId)
        {
            // Récupérer les résultats d'un tournoi en incluant les joueurs et les tournois associés
            return await Context.Results
                .Where(r => r.Tournaments.Any(t => t.Id == tournamentId))
                .Include(r => r.Players) // Charger les joueurs associés
                .Include(r => r.Tournaments) // Charger les tournois associés
                .ToListAsync();
        }
        public async Task<List<ResultEntity>> GetFullResultsByTournamentAsync(TournamentEntity tournament)
        {
            // Réutiliser la méthode GetFullResultsByTournamentIdAsync en utilisant l'ID du tournoi
            return await GetFullResultsByTournamentIdAsync(tournament.Id);
        }


        public void Dispose()
        {
            Context.Dispose();
        }
    }
}