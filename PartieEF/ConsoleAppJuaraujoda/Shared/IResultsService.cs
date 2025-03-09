// Shared/IResultsService.cs
using Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace Shared
{
    public interface IResultsService<TPlayer, TTournament> : IDisposable
         where TPlayer : class
         where TTournament : class
    {
        Task<List<ResultEntity>> GetAllAsync(Expression<Func<ResultEntity, bool>>? predicate = null, string? includeProperties = null);
        Task<ResultEntity?> GetByIdAsync(int id);
        Task<ResultEntity> AddAsync(ResultEntity result);
        Task UpdateAsync(ResultEntity result);
        Task DeleteAsync(int id);

        // Méthodes spécifiques aux résultats (Partie 2a et 3b)
        Task<List<ResultEntity>> GetResultsByPlayerIdAsync(int playerId);
        Task<List<ResultEntity>> GetResultsByTournamentIdAsync(int tournamentId);
        Task<List<ResultEntity>> GetResultsByPlayerAsync(TPlayer player); //Pour le results manager
        Task<List<ResultEntity>> GetResultsByTournamentAsync(TTournament tournament); //Pour le results manager
        Task AddResultToPlayerAsync(int playerId, int tournamentId, ResultEntity result);
        Task RemoveResultFromPlayerAsync(int playerId, int tournamentId, int resultId);

        // NOUVELLES MÉTHODES POUR LA PARTIE 3B
        Task<List<ResultEntity>> GetFullResultsByTournamentIdAsync(int tournamentId); // Récupérer les résultats complets par tournoi
        Task<List<ResultEntity>> GetFullResultsByTournamentAsync(TTournament tournament); // Récupérer les résultats complets par tournoi (Modèle)
    }
}