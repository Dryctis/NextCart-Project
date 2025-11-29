using Microsoft.EntityFrameworkCore;
using NexCart.Application.Common.Models;

namespace NexCart.Application.Common.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedList<T>> ToPagedListAsync<T>(
        this IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var count = await source.CountAsync(cancellationToken);

        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedList<T>(items, pageNumber, pageSize, count);
    }

    public static IQueryable<T> ApplyPaging<T>(
        this IQueryable<T> source,
        PaginationParams paginationParams)
    {
        return source
            .Skip(paginationParams.Skip)
            .Take(paginationParams.Take);
    }
}