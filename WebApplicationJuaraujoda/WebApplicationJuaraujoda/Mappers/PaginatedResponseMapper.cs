using Dto;
using Entities;
using System.Collections.Generic;

namespace WtaApi.Mappers
{
    public static class PaginatedResponseMapper
    {
        public static PaginatedResponseDto<T> ToDto<T>(PaginatedResponse<T> paginatedResponse)
        {
            if (paginatedResponse == null)
                return null;

            return new PaginatedResponseDto<T>
            {
                TotalCount = paginatedResponse.TotalCount,
                PageIndex = paginatedResponse.PageIndex,
                CountPerPage = paginatedResponse.CountPerPage,
                Items = paginatedResponse.Items
            };
        }

        public static PaginatedResponse<T> ToEntity<T>(PaginatedResponseDto<T> dto)
        {
            if (dto == null)
                return null;

            return new PaginatedResponse<T>
            {
                TotalCount = dto.TotalCount,
                PageIndex = dto.PageIndex,
                CountPerPage = dto.CountPerPage,
                Items = dto.Items
            };
        }
    }
}