using Entities;
using Model;
using System;
using System.Linq;

namespace Model2Entities
{
    public static class TournamentMapper
    {
        public static Tournament ToModel(this TournamentEntity entity)
        {
            if (entity == null) return null;

            return new Tournament(
                entity.Id,
                entity.Name,
                entity.Year
            );
        }

        public static TournamentEntity ToEntity(this Tournament model)
        {
            if (model == null) return null;

            return new TournamentEntity
            {
                Id = model.Id,
                Name = model.Name,
                Year = model.Year
            };
        }
        public static List<Tournament> ToModelList(this IEnumerable<TournamentEntity> entities)
        {
            return entities.Select(e => e.ToModel()).ToList();
        }
        public static List<TournamentEntity> ToEntityList(this IEnumerable<Tournament> models)
        {
            return models.Select(m => m.ToEntity()).ToList();
        }
    }
}