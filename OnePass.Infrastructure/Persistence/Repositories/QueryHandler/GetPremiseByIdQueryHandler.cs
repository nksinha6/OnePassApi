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
    public class GetPremiseByIdQueryHandler : QueryHandlerBase<GetPremiseByIdQuery, PremiseResponse>,
                                              IReadQueryHandler<GetPremiseByIdQuery, PremiseResponse>
    {
        // compiled query that joins premise & premise_type and projects to PremiseResponse
        private static readonly Func<OnePassDbContext, Guid, Task<PremiseResponse?>> _getPremiseByIdQuery =
            EF.CompileAsyncQuery((OnePassDbContext ctx, Guid id) =>
                ctx.Premises
                   .AsNoTracking()
                   .Where(p => p.Id == id)
                   .Join(ctx.PremiseTypes,
                         p => p.TypeId,
                         pt => pt.Id,
                         (p, pt) => new PremiseResponse
                         {
                             Id = p.Id,
                             Name = p.Name,
                             TenantId = p.TenantId,
                             TypeId = p.TypeId,
                             Type = pt.Name,
                             ParentId = p.ParentId,
                             Latitude = p.Latitude,
                             Longitude = p.Longitude,
                             Address = p.Address,
                             City = p.City,
                             Zip = p.Zip,
                             State = p.State,
                             Country = p.Country,
                             GmapUrl = p.GmapUrl,
                             Admin = p.Admin
                         })
                   .FirstOrDefault());

        public GetPremiseByIdQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetPremiseByIdQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<PremiseResponse>> HandleQueryAsync(GetPremiseByIdQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
            {
                var premise = await _getPremiseByIdQuery(ctx, query.Id);
                if (premise == null)
                    return Enumerable.Empty<PremiseResponse>();  // or throw if you prefer
                return new List<PremiseResponse> { premise };
            });
        }

        public Task<IEnumerable<PremiseResponse>> HandleAllAsync()
        {
            throw new InvalidOperationException("Fetching all premises is not supported in this handler.");
        }
    }
}
