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
    public class GetPremisesByParentIdQueryHandler :
    QueryHandlerBase<GetPremisesByParentIdQuery, PremiseResponse>,
    IReadQueryHandler<GetPremisesByParentIdQuery, PremiseResponse>
    {
        // compiled query that joins premise, premise_type, tenant
        private static readonly Func<OnePassDbContext, Guid, IAsyncEnumerable<PremiseResponse>> _getPremisesByParentIdQuery =
            EF.CompileAsyncQuery((OnePassDbContext ctx, Guid parentId) =>
                ctx.Premises
                    .AsNoTracking()
                    .Where(p => p.ParentId == parentId)
                    .Join(ctx.PremiseTypes,
                          p => p.TypeId,
                          pt => pt.Id,
                          (p, pt) => new { p, pt })
                    .OrderBy(x => x.p.Zip)
                    .ThenBy(x => x.pt.Name)
                    .Select(x => new PremiseResponse
                    {
                        Id = x.p.Id,
                        Name = x.p.Name,
                        TenantId = x.p.TenantId,
                        TypeId = x.p.TypeId,
                        Type = x.pt.Name,
                        ParentId = x.p.ParentId,
                        Latitude = x.p.Latitude,
                        Longitude = x.p.Longitude,
                        Address = x.p.Address,
                        City = x.p.City,
                        Zip = x.p.Zip,
                        State = x.p.State,
                        Country = x.p.Country,
                        GmapUrl = x.p.GmapUrl,
                        Admin = x.p.Admin
                    }));


        public GetPremisesByParentIdQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetPremisesByParentIdQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<PremiseResponse>> HandleQueryAsync(GetPremisesByParentIdQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
            {
                var premises = await _getPremisesByParentIdQuery(ctx, query.ParentId).ToListAsync();
                return premises;
            });
        }

        public Task<IEnumerable<PremiseResponse>> HandleAllAsync()
        {
            throw new InvalidOperationException("Fetching all premises without parentId is not supported in this handler.");
        }
    }
}
