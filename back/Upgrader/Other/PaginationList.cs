using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;

namespace Upgrader.Other;

public class PaginationList<T>
{
    public ReadOnlyCollection<T> Items { get; private set; }
    public int PagesCount { get; private set; }
    public int Page { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }

    public PaginationList(List<T> items, int pagesCount, int page, int pageSize, int totalCount)
    {
        Items = items.AsReadOnly();
        PagesCount = pagesCount;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public static async Task<PaginationList<T>> CreateFromQuery(
        IQueryable<T> query,
        int pageSize,
        int page
    )
    {
        var totalCount = await query.CountAsync();
        var pagesCount = CountPages(totalCount, pageSize);

        var items = await PaginateQuery(query, page, pageSize).ToListAsync();

        return new PaginationList<T>(items, pagesCount, page, pageSize, totalCount);
    }

    public static async Task<PaginationList<K>> CreateFromQueryWithAfterTransform<K>(
        IQueryable<T> query,
        int pageSize,
        int page,
        Func<T, int, K> transform
    )
    {
        var totalCount = await query.CountAsync();
        var pagesCount = CountPages(totalCount, pageSize);

        var items = await PaginateQuery(query, page, pageSize).ToListAsync();

        var transformedItems = items.Select((item, index) => transform(item, index)).ToList();

        return new PaginationList<K>(transformedItems, pagesCount, page, pageSize, totalCount);
    }

    private static IQueryable<T> PaginateQuery(IQueryable<T> query, int page, int pageSize)
    {
        var skipCount = (page - 1) * pageSize;

        return query.Skip(skipCount).Take(pageSize);
    }

    private static int CountPages(int totalCount, int pageSize)
    {
        var pagesCount = (int)Math.Ceiling((double)totalCount / pageSize);

        if (pagesCount == 0)
            pagesCount = 1;

        return pagesCount;
    }
}