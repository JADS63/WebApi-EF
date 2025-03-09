using System.Collections.Generic;

namespace Dto
{
    public class PaginatedResponseDto<T>
    {
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int CountPerPage { get; set; }
        public IEnumerable<T>? Items { get; set; }
    }
}