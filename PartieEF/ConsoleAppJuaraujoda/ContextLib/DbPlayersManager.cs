// ContextLib/DbPlayersManager.cs
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
    public class DbPlayersManager : IPlayersService<PlayerEntity>
    {
        private WtaContext Context { get; set; }

        public DbPlayersManager(WtaContext context)
        {
            Context = context;
        }

        public async Task<List<PlayerEntity>> GetAllAsync(Expression<Func<PlayerEntity, bool>>? predicate = null, string? includeProperties = null)
        {
            IQueryable<PlayerEntity> query = Context.Players;

            if (predicate != null)
            {
                // WORKAROUND - CLIENT-SIDE EVALUATION (pour éviter l'erreur de traduction LINQ)
                query = query.Where(predicate).AsEnumerable().AsQueryable();
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

        public async Task<PlayerEntity?> GetByIdAsync(int id)
        {
            return await Context.Players.FindAsync(id);
        }

        public async Task<PlayerEntity> AddAsync(PlayerEntity player)
        {
            Context.Players.Add(player);
            await Context.SaveChangesAsync();
            return player;
        }

        public async Task UpdateAsync(PlayerEntity player)
        {
            // VERSION MODIFIÉE DE UPDATEASYNC - RÉCUPÉRER L'ENTITÉ SUIVIE ET MODIFIER
            var existingPlayer = await Context.Players.FindAsync(player.Id); // Récupérer l'entité suivie par son ID
            if (existingPlayer != null)
            {
                // Mettre à jour les propriétés de l'entité existante avec les valeurs de l'entité 'player'
                Context.Entry(existingPlayer).CurrentValues.SetValues(player);
                await Context.SaveChangesAsync(); // Enregistrer les changements sur l'entité suivie
            }
            // Si existingPlayer est null, l'entité à mettre à jour n'a pas été trouvée (gérer selon vos besoins)
        }

        public async Task DeleteAsync(int id)
        {
            var player = await Context.Players.FindAsync(id);
            if (player != null)
            {
                Context.Players.Remove(player);
                await Context.SaveChangesAsync();
            }
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}