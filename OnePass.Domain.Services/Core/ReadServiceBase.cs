using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain.Services
{
    public abstract class ReadServiceBase
    {
        private readonly IReadRepositoryFactory _repositoryFactory;

        protected ReadServiceBase(IReadRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        protected IReadRepository<TQuery, TResult> GetRepository<TQuery, TResult>(bool useStoredProcedure)
            where TQuery : IReadQuery
            where TResult : class, new()
        {
            return _repositoryFactory.GetRepository<TQuery, TResult>(useStoredProcedure);
        }

        private async Task<T> WithRepository<TQuery, TResult, T>(
            bool useStoredProcedure,
            Func<IReadRepository<TQuery, TResult>, Task<T>> handleRepo)
            where TQuery : IReadQuery
            where TResult : class, new()
        {
            var repository = GetRepository<TQuery, TResult>(useStoredProcedure);

            if (repository == null)
                throw new InvalidOperationException("Failed to retrieve repository");

            return await handleRepo(repository);
        }

        protected async Task<IEnumerable<TResult>> HandleQueryAsync<TQuery, TResult>(
            TQuery query,
            bool useStoredProcedure = true)
            where TQuery : IReadQuery
            where TResult : class, new()
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query), "Query must not be null");

            return await WithRepository<TQuery, TResult, IEnumerable<TResult>>(useStoredProcedure,
                async repo =>
                {
                    var items = await repo.ExecuteQueryAsync(query);
                    return items;
                });
        }

        protected async Task<IEnumerable<TResult>> HandleAllAsync<TQuery, TResult>(
            bool useStoredProcedure = true,
            string notFoundMessage = null)
            where TQuery : IReadQuery, new()
            where TResult : class, new()
        {
            return await WithRepository<TQuery, TResult, IEnumerable<TResult>>(useStoredProcedure,
                async repo =>
                {
                    var items = await repo.ExecuteAllAsync();
                    if (!items.Any())
                        throw new InvalidOperationException(notFoundMessage ?? $"No entries found for query: {typeof(TQuery).Name}");

                    return items;
                });
        }

        protected async Task<TResult> HandleSingleOrDefaultAsync<TQuery, TResult>(
            TQuery query,
            bool useStoredProcedure = true,
            string notFoundMessage = null)
            where TQuery : IReadQuery
            where TResult : class, new()
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query), "Query must not be null");

            return await WithRepository<TQuery, TResult, TResult>(useStoredProcedure,
                async repo =>
                {
                    var items = await repo.ExecuteQueryAsync(query);
                    return items.Count() > 0 ? items.Single() : new TResult();
                });
        }
    }
}
