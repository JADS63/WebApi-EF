// ContextLib/DbTournamentsManager.cs
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
    public class DbTournamentsManager : ITournamentsService<TournamentEntity>
    {
        private WtaContext Context { get; set; }

        public DbTournamentsManager(WtaContext context)
        {
            Context = context;
        }

        public async Task<List<TournamentEntity>> GetAllAsync(Expression<Func<TournamentEntity, bool>>? predicate = null, string? includeProperties = null)
        {
            IQueryable<TournamentEntity> query = Context.Tournaments;
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

        public async Task<TournamentEntity?> GetByIdAsync(int id)
        {
            return await Context.Tournaments.FindAsync(id);
        }

        public async Task<TournamentEntity> AddAsync(TournamentEntity tournament)
        {
            Context.Tournaments.Add(tournament);
            await Context.SaveChangesAsync();
            return tournament;
        }

        public async Task UpdateAsync(TournamentEntity tournament)
        {
            Context.Entry(tournament).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tournament = await Context.Tournaments.FindAsync(id);
            if (tournament != null)
            {
                Context.Tournaments.Remove(tournament);
                await Context.SaveChangesAsync();
            }
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}