using AutoMapper;
using bg.crm.integration.application.dtos.models.pagination;
using Microsoft.EntityFrameworkCore;

namespace bg.crm.integration.infrastructure.extensions
{
    public static class DataPagerExtensions
    {
        public static async Task<PaginationFilterResponse<T>> PaginateAsync<S,T>(
            this IQueryable<S> query,
            int startRow,
            int limit,
            IMapper mapper,
            CancellationToken cancellationToken = default)
        {
            PaginationFilterResponse<T> response = new PaginationFilterResponse<T>();
            response.pagination.Limit = limit;
            List<S> consulta = await query.Skip(startRow).Take(limit).ToListAsync();
            response.data = mapper.Map<List<T>>(consulta);
            int totalCount = await query.CountAsync(cancellationToken);
            response.pagination.Total = totalCount;
            response.pagination.Returned = consulta.Count;
            response.pagination.Offset = startRow;
            return response;
        }

        public static async Task<PaginationFilterResponse<T>> PaginateAsync<T>(
            this IQueryable<T> query,
            int startRow,
            int limit,
            IMapper mapper,
            CancellationToken cancellationToken = default)
        {
            PaginationFilterResponse<T> response = new PaginationFilterResponse<T>();
            response.pagination.Limit = limit;
            List<T> consulta = await query.Skip(startRow).Take(limit).ToListAsync();
            response.data = consulta;
            int totalCount = await query.CountAsync(cancellationToken);
            response.pagination.Total = totalCount;
            response.pagination.Returned = consulta.Count;
            response.pagination.Offset = startRow;
            return response;
        }

        public static async Task<PaginationFilterResponse<T>> PaginateAsync<T>(
            this IQueryable<T> query,
            int startRow,
            int limit,
            CancellationToken cancellationToken = default)
        {
            PaginationFilterResponse<T> response = new PaginationFilterResponse<T>();
            response.pagination.Limit = limit;
            List<T> consulta = await query.Skip(startRow).Take(limit).ToListAsync(cancellationToken);
            response.data = consulta;
            int totalCount = await query.CountAsync(cancellationToken);
            response.pagination.Total = totalCount;
            response.pagination.Returned = consulta.Count;
            response.pagination.Offset = startRow;
            return response;
        }
    }
}