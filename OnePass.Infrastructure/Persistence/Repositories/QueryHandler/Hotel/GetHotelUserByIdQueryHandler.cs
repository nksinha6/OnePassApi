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
    public class GetHotelUserByIdQueryHandler :
    QueryHandlerBase<GetHotelUserByIdQuery, HotelUserResponse>,
    IReadQueryHandler<GetHotelUserByIdQuery, HotelUserResponse>
    {
        private static readonly Func<OnePassDbContext, string, int, Task<HotelUserResponse>> GetHotelUserByIdCompiledQuery =
            EF.CompileAsyncQuery((OnePassDbContext ctx, string userId, int tenantId) =>
                (from u in ctx.HotelUsers.AsNoTracking()
                 join t in ctx.HotelTenants.AsNoTracking() on u.TenantId equals t.Id
                 where u.Id == userId && u.TenantId == tenantId
                 select new HotelUserResponse
                 {
                     Id = u.Id,
                     TenantId = u.TenantId,
                     TenantName = t.Name,
                     AadharStatus = u.AadharStatus.ToString()
                 })
                .FirstOrDefault());

        public GetHotelUserByIdQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetHotelUserByIdQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<HotelUserResponse>> HandleAllAsync()
        {
            throw new NotSupportedException("Fetching all hotel users is not supported in this query.");
        }

        public async Task<IEnumerable<HotelUserResponse>> HandleQueryAsync(GetHotelUserByIdQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
            {
                var result = await GetHotelUserByIdCompiledQuery(ctx, query.Id, query.TenantId);
                return result != null ? new List<HotelUserResponse> { result } : Enumerable.Empty<HotelUserResponse>();
            });
        }
    }

}
