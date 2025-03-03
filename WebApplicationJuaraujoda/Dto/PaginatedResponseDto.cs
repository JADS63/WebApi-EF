using System.Collections.Generic;

namespace Dto
{
    /// <summary>
    /// DTO pour une réponse paginée.
    /// </summary>
    /// <typeparam name="T">Type des éléments de la collection.</typeparam>
    public class PaginatedResponseDto<T>
    {
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int CountPerPage { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
