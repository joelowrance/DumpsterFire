using Microsoft.EntityFrameworkCore;

namespace MegaMercado.Application.Common;

public class PagedList<T>
{
    public PagedList(List<T> items)
    {
        Items = items;
    }

    public List<T> Items { get; set; }
    
    public int TotalCount { get; set; }
    
    public int PageIndex { get; set; }
    
    public int PageSize { get; set; }
    
    public int TotalPages => (int) Math.Ceiling(TotalCount / (double) PageSize);
    
    public bool HasPreviousPage => PageIndex > 1;
    
    public bool HasNextPage => PageIndex < TotalPages;
    
    public int? NextPageNumber => HasNextPage ? PageIndex + 1 : null;
    
    public int? PreviousPageNumber => HasPreviousPage ? PageIndex - 1 : null;
    
    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = source.Count();
        var items = await source
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return new PagedList<T>(items)
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalCount = count
        };
    }
}