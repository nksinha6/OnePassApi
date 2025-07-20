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
    public class GetUnitsByCompanyIdQueryHandler :
    QueryHandlerBase<GetUnitsByCompanyIdQuery, UnitResponse>,
    IReadQueryHandler<GetUnitsByCompanyIdQuery, UnitResponse>
    {
        private static readonly Func<OnePassDbContext, Guid, IAsyncEnumerable<UnitResponse>> GetUnitsByCompanyIdQuery =
            EF.CompileAsyncQuery((OnePassDbContext ctx, Guid companyId) =>
                from u in ctx.Units.AsNoTracking()
                join c in ctx.Companies.AsNoTracking() on u.CompanyId equals c.Id
                join p in ctx.Properties.AsNoTracking() on u.PropertyId equals p.Id into props
                from p in props.DefaultIfEmpty()
                where u.CompanyId == companyId
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

        public GetUnitsByCompanyIdQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetUnitsByCompanyIdQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<UnitResponse>> HandleQueryAsync(GetUnitsByCompanyIdQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
            {
                return await GetUnitsByCompanyIdQuery(ctx, query.CompanyId).ToListAsync();
            });
        }

        public Task<IEnumerable<UnitResponse>> HandleAllAsync()
        {
            throw new NotSupportedException("Fetching all units without companyId is not supported.");
        }
    }

}
