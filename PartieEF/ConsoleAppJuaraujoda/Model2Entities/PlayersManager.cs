// Model2Entities/PlayersManager.cs
using Model;
using Shared;
using Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Numerics;

namespace Model2Entities
{
    public class PlayersManager : IPlayersService<Player>
    {
        private readonly IPlayersService<PlayerEntity> _entityService;

        public PlayersManager(IPlayersService<PlayerEntity> entityService)
        {
            _entityService = entityService;
        }

        public async Task<List<Player>> GetAllAsync(Expression<Func<Player, bool>>? predicate = null, string? includeProperties = null)
        {
            Expression<Func<PlayerEntity, bool>>? entityPredicate = null;
            if (predicate != null)
            {
                entityPredicate = null; 

            }

            var entities = await _entityService.GetAllAsync(entityPredicate, includeProperties); 
            return entities.ToModelList();
        }

        public async Task<Player?> GetByIdAsync(int id)
        {
            var entity = await _entityService.GetByIdAsync(id);
            return entity.ToModel();
        }

        public async Task<Player> AddAsync(Player player)
        {
            var entity = player.ToEntity();
            var addedEntity = await _entityService.AddAsync(entity);
            return addedEntity.ToModel();
        }

        public async Task UpdateAsync(Player player)
        {
            var entity = player.ToEntity();
            await _entityService.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _entityService.DeleteAsync(id);
        }

        public void Dispose()
        {
            _entityService.Dispose();
        }
    }
}