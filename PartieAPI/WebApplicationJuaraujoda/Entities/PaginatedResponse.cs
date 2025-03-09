using System.Collections.Generic;

namespace Entities
{
    public class PaginatedResponse<T>
    {
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int CountPerPage { get; set; }
        public IEnumerable<T>? Items { get; set; }
    }
}