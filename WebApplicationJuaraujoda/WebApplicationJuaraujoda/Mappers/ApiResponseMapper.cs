using Dto;
using Entities;
namespace WtaApi.Mappers
{
    public static class ApiResponseMapper
    {
        public static ApiResponseDto<T> ToDto<T>(ApiResponse<T> apiResponse)
        {
            if (apiResponse == null)
                return null;

            return new ApiResponseDto<T>
            {
                Result = apiResponse.Result,
                Id = apiResponse.Id,
                Exception = apiResponse.Exception,
                Status = apiResponse.Status,
                IsCanceled = apiResponse.IsCanceled,
                IsCompleted = apiResponse.IsCompleted,
                IsCompletedSuccessfully = apiResponse.IsCompletedSuccessfully,
                CreationOptions = apiResponse.CreationOptions,
                AsyncState = apiResponse.AsyncState,
                IsFaulted = apiResponse.IsFaulted
            };
        }

        public static ApiResponse<T> ToEntity<T>(ApiResponseDto<T> dto)
        {
            if (dto == null)
                return null;

            return new ApiResponse<T>
            {
                Result = dto.Result,
                Id = dto.Id,
                Exception = dto.Exception,
                Status = dto.Status,
                IsCanceled = dto.IsCanceled,
                IsCompleted = dto.IsCompleted,
                IsCompletedSuccessfully = dto.IsCompletedSuccessfully,
                CreationOptions = dto.CreationOptions,
                AsyncState = dto.AsyncState,
                IsFaulted = dto.IsFaulted
            };
        }
    }
}