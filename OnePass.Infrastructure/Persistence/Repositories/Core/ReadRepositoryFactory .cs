using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OnePass.Domain;

namespace OnePass.Infrastructure.Persistence
{
    public class ReadRepositoryFactory : IReadRepositoryFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ReadRepositoryFactory> _logger;

        public ReadRepositoryFactory(IServiceProvider serviceProvider, ILogger<ReadRepositoryFactory> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public IReadRepository<TQuery, TResult> GetRepository<TQuery, TResult>(bool useStoredProcedure)
            where TResult : class, new()
            where TQuery : IReadQuery
        {
            try
            {
                var service = useStoredProcedure
                    ? _serviceProvider.GetRequiredService<IStoredProcReadRepository<TQuery, TResult>>()
                    : _serviceProvider.GetRequiredService<IReadRepository<TQuery, TResult>>();

                return service;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Failed to resolve repository for query type {QueryType}", typeof(TQuery).Name);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while selecting repository for query type {QueryType}", typeof(TQuery).Name);
                throw;
            }
        }
    }
}
