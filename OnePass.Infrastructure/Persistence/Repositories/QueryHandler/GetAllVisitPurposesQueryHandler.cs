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
    public class GetAllVisitPurposesQueryHandler :
        QueryHandlerBase<GetAllVisitPurposesQuery, VisitPurpose>,
        IReadQueryHandler<GetAllVisitPurposesQuery, VisitPurpose>
    {
        private static readonly Func<OnePassDbContext, IAsyncEnumerable<VisitPurpose>> GetAllVisitPurposesCompiledQuery =
            EF.CompileAsyncQuery((OnePassDbContext ctx) =>
                from vp in ctx.VisitPurposes.AsNoTracking()
                orderby vp.Name
                select new VisitPurpose
                {
                    Id = vp.Id,
                    Name = vp.Name,
                    Description = vp.Description,
                    CreatedAt = vp.CreatedAt
                });

        public GetAllVisitPurposesQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetAllVisitPurposesQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<VisitPurpose>> HandleQueryAsync(GetAllVisitPurposesQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
            {
                return await GetAllVisitPurposesCompiledQuery(ctx).ToListAsync();
            });
        }

        public Task<IEnumerable<VisitPurpose>> HandleAllAsync()
        {
            throw new NotSupportedException("HandleAllAsync is not supported for GetAllVisitPurposesQuery.");
        }
    }
}
