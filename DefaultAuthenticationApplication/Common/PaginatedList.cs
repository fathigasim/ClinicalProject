using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Common
{
    public class PaginatedList<T>
    {
        public IReadOnlyList<T> Items { get; set; } = new List<T>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        // [JsonIgnore] // Computed property - don't serialize
        public int TotalPages => PageSize > 0
            ? (int)Math.Ceiling(TotalCount / (double)PageSize)
            : 0;

        // Parameterless constructor
        public PaginatedList()
        {
            Items = new List<T>();
        }

        //  Full constructor
        //[JsonConstructor]
        public PaginatedList(IReadOnlyList<T> items, int pageNumber, int pageSize, int totalCount)
        {
            Items = items ?? new List<T>();
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
        }
    }
}


