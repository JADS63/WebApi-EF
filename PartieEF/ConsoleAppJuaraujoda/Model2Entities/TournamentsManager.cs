using Model;
using Shared;
using Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Model2Entities
{
    public class TournamentsManager : ITournamentsService<Tournament>
    {
        private readonly ITournamentsService<TournamentEntity> _entityService;

        public TournamentsManager(ITournamentsService<TournamentEntity> entityService)
        {
            _entityService = entityService;
        }

        public async Task<List<Tournament>> GetAllAsync(Expression<Func<Tournament, bool>>? predicate = null, string? includeProperties = null)
        {
            Expression<Func<TournamentEntity, bool>>? entityPredicate = null;
            if (predicate != null)
            {
                entityPredicate = Expression.Lambda<Func<TournamentEntity, bool>>(
                    predicate.Body,
                    Expression.Parameter(typeof(TournamentEntity), predicate.Parameters[0].Name)
                );
            }

            var entities = await _entityService.GetAllAsync(entityPredicate, includeProperties);
            return entities.ToModelList();
        }

        public async Task<Tournament?> GetByIdAsync(int id)
        {
            var entity = await _entityService.GetByIdAsync(id);
            return entity.ToModel();
        }

        public async Task<Tournament> AddAsync(Tournament tournament)
        {
            var entity = tournament.ToEntity();
            var addedEntity = await _entityService.AddAsync(entity);
            return addedEntity.ToModel();
        }

        public async Task UpdateAsync(Tournament tournament)
        {
            var entity = tournament.ToEntity();
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