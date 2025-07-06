using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OnePass.Domain;

namespace OnePass.Infrastructure.Persistence
{
    public class EfReadRepository<TQuery, TResult> : IReadRepository<TQuery, TResult>
        where TResult : class, new()
        where TQuery : IReadQuery
    {
        private static readonly ConcurrentDictionary<(Type, Type), Type> _handlerTypeCache = new();

        protected readonly IServiceProvider _provider;

        public EfReadRepository(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task<IEnumerable<TResult>> ExecuteQueryAsync(TQuery query)
        {
            var handler = ResolveHandler(_provider);
            return await handler.HandleQueryAsync(query);
        }

        public async Task<IEnumerable<TResult>> ExecuteAllAsync()
        {
            var handler = ResolveHandler(_provider);
            return await handler.HandleAllAsync();
        }

        private Type GetHandlerType()
        {
            var key = (typeof(TQuery), typeof(TResult));
            return _handlerTypeCache.GetOrAdd(key, _ =>
                typeof(IReadQueryHandler<,>).MakeGenericType(typeof(TQuery), typeof(TResult))
            );
        }

        private IReadQueryHandler<TQuery, TResult> ResolveHandler(IServiceProvider provider)
        {
            var handlerType = GetHandlerType();
            return (IReadQueryHandler<TQuery, TResult>)provider.GetRequiredService(handlerType);
        }
    }
}
