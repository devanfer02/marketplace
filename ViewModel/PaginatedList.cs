using Microsoft.EntityFrameworkCore;

namespace Marketplace.ViewModel
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = count;
            AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public static PaginatedList<T> Create(ICollection<T> source, int pageIndex, int pageSize)
        {
            var totalPages = (int)Math.Ceiling(source.Count() / (double)pageSize);
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, totalPages, pageIndex, pageSize);
        }
    }
}
