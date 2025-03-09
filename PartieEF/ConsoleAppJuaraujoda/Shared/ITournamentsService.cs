using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shared
{
    public interface ITournamentsService<TTournament> : IDisposable where TTournament : class
    {
        Task<List<TTournament>> GetAllAsync(Expression<Func<TTournament, bool>>? predicate = null, string? includeProperties = null);
        Task<TTournament?> GetByIdAsync(int id);
        Task<TTournament> AddAsync(TTournament tournament);
        Task UpdateAsync(TTournament tournament);
        Task DeleteAsync(int id);
    }
}