using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OnePass.Domain;
using OpenTelemetry.Trace;

namespace OnePass.Infrastructure.Persistence
{
    public abstract class QueryHandlerBase<TQuery, TResult> : HandlerBase
        where TQuery : IReadQuery, new()
        where TResult : class, new()
    {
        protected readonly OnePassDbContext _context;

        protected QueryHandlerBase(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<QueryHandlerBase<TQuery, TResult>> logger)
            : base(tracer, logger)
        {
            _context = context;
        }

        // Generalized method to execute queries with multiple parameters
        protected async Task<IEnumerable<TResult>> ExecuteQuerySafelyAsync(
            Func<OnePassDbContext, TQuery, Task<IEnumerable<TResult>>> queryFunc,
            TQuery query)
        {
            return await ExecuteSafelyWithTracingAsync(query, async () =>
            {
                var result = await queryFunc(_context, query).ConfigureAwait(false);
                return result;
            });
        }

        protected async Task<IEnumerable<TResult>> ExecuteQuerySafelyAsync(
            Func<OnePassDbContext, Task<IEnumerable<TResult>>> queryFunc)
        {
            return await ExecuteSafelyWithTracingAsync(new TQuery(), async () =>
            {
                var result = await queryFunc(_context).ConfigureAwait(false);
                return result;
            });
        }
    }
}
