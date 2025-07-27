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
    public class GetDesksByCompanyIdQueryHandler :
    QueryHandlerBase<GetDesksByCompanyIdQuery, DeskResponse>,
    IReadQueryHandler<GetDesksByCompanyIdQuery, DeskResponse>
    {
        private static readonly Func<OnePassDbContext, Guid, IAsyncEnumerable<DeskResponse>> GetDesksByCompanyIdQuery =
            EF.CompileAsyncQuery((OnePassDbContext ctx, Guid companyId) =>
                from d in ctx.Desks.AsNoTracking()
                join u in ctx.Units.AsNoTracking() on d.UnitId equals u.Id
                join c in ctx.Companies.AsNoTracking() on u.CompanyId equals c.Id
                join am in ctx.AccessModes.AsNoTracking() on d.AccessModeId equals am.Id
                join ac in ctx.AccessCategories.AsNoTracking() on d.AccessCategoryId equals ac.Id
                where u.CompanyId == companyId
                orderby d.Name
                select new DeskResponse
                {
                    Id = d.Id,
                    Name = d.Name,
                    UnitId = d.UnitId,
                    UnitName = u.Name,
                    AdminPhone = d.AdminPhone,

                    // ✅ Get Access Mode & Category Names
                    AccessModeId = d.AccessModeId,
                    AccessMode = am.Name,
                    AccessCategoryId = d.AccessCategoryId,
                    AccessCategory = ac.Name,

                    CompanyId = c.Id,
                    CompanyName = c.Name
                });

        public GetDesksByCompanyIdQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetDesksByCompanyIdQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<DeskResponse>> HandleQueryAsync(GetDesksByCompanyIdQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
            {
                return await GetDesksByCompanyIdQuery(ctx, query.CompanyId).ToListAsync();
            });
        }

        public Task<IEnumerable<DeskResponse>> HandleAllAsync()
        {
            throw new NotSupportedException("Fetching all desks without companyId is not supported.");
        }
    }

}
