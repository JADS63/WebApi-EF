using Dto;
using Entities;

namespace WtaApi.Mappers
{
    public static class PaginationMapper
    {
        public static PaginationDto ToDto(Pagination pagination)
        {
            if (pagination == null)
                return null;

            return new PaginationDto
            {
                Index = pagination.Index,
                Count = pagination.Count
            };
        }

        public static Pagination ToEntity(PaginationDto dto)
        {
            if (dto == null)
                return null;

            return new Pagination
            {
                Index = dto.Index,
                Count = dto.Count
            };
        }
    }
}