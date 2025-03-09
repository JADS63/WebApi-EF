using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shared
{
    public interface IPlayersService<TPlayer> : IDisposable where TPlayer : class
    {
        Task<List<TPlayer>> GetAllAsync(Expression<Func<TPlayer, bool>>? predicate = null, string? includeProperties = null);
        Task<TPlayer?> GetByIdAsync(int id);
        Task<TPlayer> AddAsync(TPlayer player);
        Task UpdateAsync(TPlayer player);
        Task DeleteAsync(int id);
    }
}