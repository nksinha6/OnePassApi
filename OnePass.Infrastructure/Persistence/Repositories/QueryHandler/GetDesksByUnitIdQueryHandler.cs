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
    public class GetDesksByUnitIdQueryHandler :
        QueryHandlerBase<GetDesksByUnitIdQuery, DeskResponse>,
        IReadQueryHandler<GetDesksByUnitIdQuery, DeskResponse>
    {
        private static readonly Func<OnePassDbContext, Guid, IAsyncEnumerable<DeskResponse>> GetDesksByUnitIdQuery =
            EF.CompileAsyncQuery((OnePassDbContext ctx, Guid unitId) =>
                from d in ctx.Desks.AsNoTracking()
                join u in ctx.Units.AsNoTracking() on d.UnitId equals u.Id
                join c in ctx.Companies.AsNoTracking() on u.CompanyId equals c.Id
                join am in ctx.AccessModes.AsNoTracking() on d.AccessModeId equals am.Id
                join ac in ctx.AccessCategories.AsNoTracking() on d.AccessCategoryId equals ac.Id
                where u.Id == unitId
                orderby d.Name
                select new DeskResponse
                {
                    Id = d.Id,
                    Name = d.Name,
                    UnitId = d.UnitId,
                    UnitName = u.Name,
                    AdminPhone = d.AdminPhone,
                    AccessModeId = d.AccessModeId,
                    AccessCategoryId = d.AccessCategoryId,
                    AccessMode = am.Name,
                    AccessCategory = ac.Name,
                    CompanyId = c.Id,
                    CompanyName = c.Name
                });

        public GetDesksByUnitIdQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetDesksByUnitIdQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<DeskResponse>> HandleQueryAsync(GetDesksByUnitIdQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
            {
                return await GetDesksByUnitIdQuery(ctx, query.UnitId).ToListAsync();
            });
        }

        public Task<IEnumerable<DeskResponse>> HandleAllAsync()
        {
            throw new NotSupportedException("Fetching all desks without unitId is not supported.");
        }
    }
}
