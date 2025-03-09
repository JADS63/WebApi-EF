using Entities;
using Model;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Model2Entities
{
    public static class PlayerMapper
    {
        public static Player ToModel(this PlayerEntity entity)
        {
            if (entity == null) return null;
            return new Player(
                entity.Id,
                entity.FirstName,
                entity.LastName,
                entity.BirthDate,
                entity.Height,
                entity.Nationality,
                entity.Handplay
            );
        }

        public static PlayerEntity ToEntity(this Player model)
        {
            if (model == null) return null;
            return new PlayerEntity
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                BirthDate = model.BirthDate,
                Height = model.Height, 
                Nationality = model.Nationality,
                Handplay = model.Handplay
            };
        }
        // Méthode d'extension pour convertir une liste d'entités en liste de modèles
        public static List<Player> ToModelList(this IEnumerable<PlayerEntity> entities)
        {
            return entities.Select(e => e.ToModel()).ToList();
        }
        // Méthode d'extension pour convertir une liste de modèles en liste d'entités
        public static List<PlayerEntity> ToEntityList(this IEnumerable<Player> models)
        {
            return models.Select(m => m.ToEntity()).ToList();
        }
    }
}