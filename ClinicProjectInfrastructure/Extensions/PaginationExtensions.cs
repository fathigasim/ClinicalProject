using ClinicProjectApplication.Common;
using ClinicProjectDomain.Common.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectInfrastructure.Extensions
{
    public static class PaginationExtensions
    {

        /// <summary>
        /// Paginates the query and returns a PagedResult.
        /// </summary>
        public static async Task<PagedResult<T>> ToPagedAsync<T>(
            this IQueryable<T> query,
            int page,
            int pageSize,
            CancellationToken ct = default)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var total = await query.CountAsync(ct);
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return new PagedResult<T>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        /// <summary>
        /// Applies one or more include paths (supports dot-notation: "Orders.Items").
        /// </summary>
        public static IQueryable<T> WithIncludes<T>(
            this IQueryable<T> query,
            params string[] includes) where T : class
        {
            foreach (var include in includes)
                query = query.Include(include);

            return query;
        }

        /// <summary>
        /// Applies one or more strongly-typed include expressions.
        /// </summary>
        public static IQueryable<T> WithIncludes<T>(
            this IQueryable<T> query,
            params Expression<Func<T, object?>>[] includes) where T : class
        {
            foreach (var include in includes)
                query = query.Include(include);

            return query;
        }

        public static IQueryable<T> WithIncludes<T>(
    this IQueryable<T> query,
    params Func<IQueryable<T>, IQueryable<T>>[] includes) where T : class
        {
            foreach (var include in includes)
                query = include(query);
            return query;
        }
        //public static PagedResult<TDto> MapTo<TEntity, TDto>(
        //this PagedResult<TEntity> paged,
        //IMapper mapper) =>
        //new()
        //{
        //    Items = mapper.Map<List<TDto>>(paged.Items),
        //    TotalCount = paged.TotalCount,
        //    Page = paged.Page,
        //    PageSize = paged.PageSize
        //};

    }
}
