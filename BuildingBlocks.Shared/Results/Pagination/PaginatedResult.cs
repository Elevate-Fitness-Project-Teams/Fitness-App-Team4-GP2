using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Shared.Results.Pagination
{
    public class PaginatedResult<T>
    {
        public IReadOnlyList<T> Items { get; init; } = [];
        public int Page { get; init; }
        public int PageSize { get; init; }
        public int TotalCount { get; init; }

        public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasNextPage => Page < TotalPages;
        public bool HasPreviousPage => Page > 1;

        public static PaginatedResult<T> Create(
            IReadOnlyList<T> items, int totalCount, int page, int pageSize) => new()
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
    }
    public record PaginationRequest(int Page = 1, int PageSize = 20)
    {
        public int Page { get; init; } = Page < 1 ? 1 : Page;
        public int PageSize { get; init; } = PageSize < 1 ? 20 : PageSize > 100 ? 100 : PageSize;
        public int Skip => (Page - 1) * PageSize;
    }
}
