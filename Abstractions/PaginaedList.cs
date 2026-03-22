using Microsoft.EntityFrameworkCore;

namespace SurveyBasket.Abstractions
{
    public class PaginaedList<T>
    {
        public PaginaedList(List<T> items, int pageNumber, int count, int Pagesize)
        {
            Items = items;
            PageNumber = pageNumber;
            TotalPages = (int) Math.Ceiling(count / (double)Pagesize);
        }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public List<T> Items { get; private set; }
        public bool HasPerviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
        
        public static async Task<PaginaedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var count = await source.CountAsync(cancellationToken);
            
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            
            return new PaginaedList<T>(items, pageNumber, count, pageSize);
        }
    }
}
