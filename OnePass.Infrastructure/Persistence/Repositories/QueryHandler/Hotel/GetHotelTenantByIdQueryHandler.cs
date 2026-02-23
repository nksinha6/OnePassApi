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
    public class GetHotelTenantByIdQueryHandler :
    QueryHandlerBase<GetHotelTenantByIdQuery, HotelTenantResponse>,
    IReadQueryHandler<GetHotelTenantByIdQuery, HotelTenantResponse>
    {
        private static readonly Func<OnePassDbContext, int, Task<HotelTenantResponse>>
            GetHotelTenantByIdCompiledQuery =
                EF.CompileAsyncQuery((OnePassDbContext ctx, int tenantId) =>
                    (from t in ctx.HotelTenants.AsNoTracking()
                     where t.Id == tenantId
                     select new HotelTenantResponse
                     {
                         Id = t.Id,
                         Name = t.Name,
                         Logo = t.Logo,
                         LogoContentType = t.LogoContentType
                     })
                    .FirstOrDefault());

        public GetHotelTenantByIdQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetHotelTenantByIdQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<HotelTenantResponse>> HandleAllAsync()
        {
            throw new NotSupportedException("Fetching all hotel tenants is not supported in this query.");
        }

        public async Task<IEnumerable<HotelTenantResponse>> HandleQueryAsync(
            GetHotelTenantByIdQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
            {
                var result = await GetHotelTenantByIdCompiledQuery(ctx, query.TenantId);
                return result != null
                    ? new List<HotelTenantResponse> { result }
                    : Enumerable.Empty<HotelTenantResponse>();
            });
        }
    }
}
