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
    public class GetUnitsByPropertyIdQueryHandler :
    QueryHandlerBase<GetUnitsByPropertyIdQuery, UnitResponse>,
    IReadQueryHandler<GetUnitsByPropertyIdQuery, UnitResponse>
    {
        private static readonly Func<OnePassDbContext, Guid, IAsyncEnumerable<UnitResponse>> GetUnitsByPropertyIdQuery =
            EF.CompileAsyncQuery((OnePassDbContext ctx, Guid propertyId) =>
                from u in ctx.Units.AsNoTracking()
                join c in ctx.Companies.AsNoTracking() on u.CompanyId equals c.Id
                join p in ctx.Properties.AsNoTracking() on u.PropertyId equals p.Id
                where u.PropertyId == propertyId
                orderby u.Name
                select new UnitResponse
                {
                    Id = u.Id,
                    CompanyId = u.CompanyId,
                    CompanyName = c.Name,
                    PropertyId = u.PropertyId,
                    PropertyName = p.Name,
                    Name = u.Name,
                    Floor = u.Floor,
                    AdminPhone = u.AdminPhone
                });

        public GetUnitsByPropertyIdQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetUnitsByPropertyIdQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<UnitResponse>> HandleQueryAsync(GetUnitsByPropertyIdQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
            {
                return await GetUnitsByPropertyIdQuery(ctx, query.PropertyId).ToListAsync();
            });
        }

        public Task<IEnumerable<UnitResponse>> HandleAllAsync()
        {
            throw new NotSupportedException("Fetching all units without propertyId is not supported.");
        }
    }

}
