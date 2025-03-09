using Entities;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace Model2Entities
{
    public static class ResultMapper
    {
        public static Result ToModel(this ResultEntity entity)
        {
            if (entity == null) return Entities.Result.Unknown; 
            return entity.Result;
        }

        public static ResultEntity ToEntity(this Result model)
        {
            return new ResultEntity { Result = model };
        }

    }
}