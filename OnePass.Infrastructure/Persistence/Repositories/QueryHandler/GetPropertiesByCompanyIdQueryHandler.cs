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
    public class GetPropertiesByCompanyIdQueryHandler :
    QueryHandlerBase<GetPropertiesByCompanyIdQuery, PropertyResponse>,
    IReadQueryHandler<GetPropertiesByCompanyIdQuery, PropertyResponse>
    {
        private static readonly Func<OnePassDbContext, Guid, IAsyncEnumerable<PropertyResponse>> GetPropertiesByCompanyIdQuery =
            EF.CompileAsyncQuery((OnePassDbContext ctx, Guid companyId) =>
                from p in ctx.Properties.AsNoTracking()
                join c in ctx.Companies.AsNoTracking() on p.CompanyId equals c.Id
                where p.CompanyId == companyId
                orderby p.Name
                select new PropertyResponse
                {
                    Id = p.Id,
                    CompanyId = p.CompanyId,
                    CompanyName = c.Name,
                    Name = p.Name,
                    Address = p.Address,
                    City = p.City,
                    Pincode = p.Pincode,
                    State = p.State,
                    Country = p.Country,
                    GmapUrl = p.GmapUrl,
                    AdminPhone = p.AdminPhone
                });

        public GetPropertiesByCompanyIdQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetPropertiesByCompanyIdQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<PropertyResponse>> HandleQueryAsync(GetPropertiesByCompanyIdQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
            {
                return await GetPropertiesByCompanyIdQuery(ctx, query.CompanyId).ToListAsync();
            });
        }

        public Task<IEnumerable<PropertyResponse>> HandleAllAsync()
        {
            throw new NotSupportedException("Fetching all properties without companyId is not supported.");
        }
    }

}
