using System.Collections.Generic;

namespace Entities
{
    /// <summary>
    /// Représente une réponse paginée dans le domaine.
    /// </summary>
    /// <typeparam name="T">Type des éléments de la collection.</typeparam>
    public class PaginatedResponse<T>
    {
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int CountPerPage { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
