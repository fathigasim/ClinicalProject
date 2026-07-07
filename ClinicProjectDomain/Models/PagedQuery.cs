using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Models
{
    public class PagedQuery
    {
        private int _pageNumber = 1;
        private int _pageSize = 10;
        private const int MaxPageSize = 100;

        public int PageNumber
        {
            get => _pageNumber;
            init => _pageNumber = value < 1 ? 1 : value;
        }

        public int PageSize
        {
            get => _pageSize;
            init => _pageSize = value > MaxPageSize ? MaxPageSize : value < 1 ? 1 : value;
        }

        /// <summary>
        /// Navigation properties to eager-load (e.g., "Orders.Items", "Address").
        /// </summary>
        public IReadOnlyList<string> Includes { get; init; } = [];

        public int Skip => (PageNumber - 1) * PageSize;
    }
}
