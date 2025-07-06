using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnePass.Domain;
using OpenTelemetry.Trace;

namespace OnePass.Infrastructure.Persistence
{
    public class GetCampusByIdQueryHandler : QueryHandlerBase<GetCampusByIdQuery, Campus>,
                                         IReadQueryHandler<GetCampusByIdQuery, Campus>
    {
        private static readonly Func<OnePassDbContext, Guid, Task<Campus>> _getCampusByIdQuery =
            EF.CompileAsyncQuery((OnePassDbContext ctx, Guid id) =>
                ctx.Set<Campus>().AsNoTracking().FirstOrDefault(c => c.Id == id));

        public GetCampusByIdQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetCampusByIdQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<Campus>> HandleQueryAsync(GetCampusByIdQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
            {
                var campus = await _getCampusByIdQuery(ctx, query.Id);
                if (campus == null)
                    return Enumerable.Empty<Campus>();  // or throw if you prefer
                return new List<Campus> { campus };
            });
        }

        public Task<IEnumerable<Campus>> HandleAllAsync()
        {
            throw new InvalidOperationException("Fetching all campuses is not supported in this handler.");
        }
    }
}
