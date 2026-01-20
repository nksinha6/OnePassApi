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
    public class GetUserPropertiesQueryHandler :
    QueryHandlerBase<GetUserPropertiesQuery, HotelUserPropertyResponse>,
    IReadQueryHandler<GetUserPropertiesQuery, HotelUserPropertyResponse>
    {
        // Compiled async query
        private static readonly Func<
    OnePassDbContext,
    string,
    IAsyncEnumerable<HotelUserPropertyResponse>
> GetUserPropertiesCompiledQuery =
EF.CompileAsyncQuery(
    (OnePassDbContext ctx, string userId) =>

        ctx.HotelUserProperties
           .AsNoTracking()
           .Where(hup => hup.UserId == userId)
           .OrderBy(hup => hup.PropertyId)
           .Select(hup => new HotelUserPropertyResponse
           {
               UserId = hup.UserId,
               TenantId = hup.TenantId,
               PropertyId = hup.PropertyId,

               // correlated subquery (EXACTLY like your sample)
               PropertyName = ctx.HotelProperties
                   .Where(p => p.Id == hup.PropertyId)
                   .Select(p => p.Name)
                   .FirstOrDefault()!
           })
);


        public GetUserPropertiesQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetUserPropertiesQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<HotelUserPropertyResponse>> HandleAllAsync()
        {
            throw new NotSupportedException(
                "Fetching all user properties without filters is not supported.");
        }

        public async Task<IEnumerable<HotelUserPropertyResponse>> HandleQueryAsync(
            GetUserPropertiesQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
                await GetUserPropertiesCompiledQuery(
                    ctx,
                    query.UserId)
                .ToListAsync());
        }
    }
}
