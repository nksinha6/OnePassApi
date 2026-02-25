using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnePass.Domain;
using OnePass.Dto;
using OpenTelemetry.Trace;

namespace OnePass.Infrastructure.Persistence
{
    public class GetHotelPropertyNameByIdQueryHandler :
    QueryHandlerBase<GetHotelPropertyNameByIdQuery, HotelPropertyNameResponse>,
    IReadQueryHandler<GetHotelPropertyNameByIdQuery, HotelPropertyNameResponse>
    {
        private static readonly Func<OnePassDbContext, int, Task<HotelPropertyNameResponse>>
            GetHotelPropertyNameByIdCompiledQuery =
                EF.CompileAsyncQuery((OnePassDbContext ctx, int propertyId) =>
                    (from p in ctx.HotelProperties.AsNoTracking()
                     where p.Id == propertyId
                     select new HotelPropertyNameResponse
                     {
                         Id = p.Id,
                         Name = p.Name,
                         TenantId = p.TenantId,
                         PropertyType = p.PropertyType,
                         Tier = p.Tier
                     })
                    .FirstOrDefault());

        public GetHotelPropertyNameByIdQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetHotelPropertyNameByIdQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<HotelPropertyNameResponse>> HandleAllAsync()
        {
            throw new NotSupportedException("Fetching all hotel properties is not supported in this query.");
        }

        public async Task<IEnumerable<HotelPropertyNameResponse>> HandleQueryAsync(
            GetHotelPropertyNameByIdQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
            {
                var result = await GetHotelPropertyNameByIdCompiledQuery(ctx, query.PropertyId);
                return result != null
                    ? new List<HotelPropertyNameResponse> { result }
                    : Enumerable.Empty<HotelPropertyNameResponse>();
            });
        }
    }

}
