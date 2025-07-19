using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnePass.Domain;
using OpenTelemetry.Trace;

namespace OnePass.Infrastructure.Persistence
{
    public class GetPremisesByTenantIdQueryHandler :
    QueryHandlerBase<GetPremisesByTenantIdQuery, PremiseResponse>,
    IReadQueryHandler<GetPremisesByTenantIdQuery, PremiseResponse>
    {
        // compiled query that joins premise, premise_type, tenant
        private static readonly Func<OnePassDbContext, Guid, IAsyncEnumerable<PremiseResponse>> _getPremisesByTenantIdQuery =
            EF.CompileAsyncQuery((OnePassDbContext ctx, Guid tenantId) =>
                ctx.Premises
                    .AsNoTracking()
                    .Where(p => p.TenantId == tenantId)
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


        public GetPremisesByTenantIdQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetPremisesByTenantIdQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<PremiseResponse>> HandleQueryAsync(GetPremisesByTenantIdQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
            {
                var premises = await _getPremisesByTenantIdQuery(ctx, query.TenantId).ToListAsync();
                return premises;
            });
        }

        public Task<IEnumerable<PremiseResponse>> HandleAllAsync()
        {
            throw new InvalidOperationException("Fetching all premises without tenantId is not supported in this handler.");
        }
    }

}
