using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnePass.Domain;
using OpenTelemetry.Trace;

namespace OnePass.Infrastructure.Persistence
{
    public class GetPendingFaceMatchesQueryHandler :
    QueryHandlerBase<GetPendingFaceMatchesQuery, HotelPendingFaceMatchDetailedResponse>,
    IReadQueryHandler<GetPendingFaceMatchesQuery, HotelPendingFaceMatchDetailedResponse>
    {
        // Compiled async query returning a List<HotelPendingFaceMatchResponse>
        private static readonly Func<
    OnePassDbContext,
    int,
    int,
    IAsyncEnumerable<HotelPendingFaceMatchDetailedResponse>
> GetPendingFaceMatchesEntitiesCompiledQuery =
EF.CompileAsyncQuery(
    (OnePassDbContext ctx, int tenantId, int propertyId) =>

        ctx.HotelPendingFaceMatches
           .AsNoTracking()
           .Where(p =>
               p.TenantId == tenantId &&
               p.PropertyId == propertyId &&
               p.Status == "pending")
           .OrderBy(p => p.CreatedAt)
           .Select(p => new HotelPendingFaceMatchDetailedResponse
           {
               Id = p.Id,
               BookingId = p.BookingId,

               TenantId = p.TenantId,
               PropertyId = p.PropertyId,

               PhoneCountryCode = p.PhoneCountryCode,
               PhoneNumber = p.PhoneNumber,

               // LEFT JOIN behavior
               FullName = ctx.HotelGuests
                   .Where(g =>
                       g.PhoneCountryCode == p.PhoneCountryCode &&
                       g.PhoneNumber == p.PhoneNumber)
                   .Select(g => g.FullName)
                   .FirstOrDefault(),

               Status = p.Status,
               CreatedAt = p.CreatedAt
           })
);




        public GetPendingFaceMatchesQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetPendingFaceMatchesQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<HotelPendingFaceMatchDetailedResponse>> HandleAllAsync()
        {
            throw new NotSupportedException("Fetching all pending face matches is not supported in this query.");
        }

        public async Task<IEnumerable<HotelPendingFaceMatchDetailedResponse>> HandleQueryAsync(GetPendingFaceMatchesQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
            await GetPendingFaceMatchesEntitiesCompiledQuery(ctx, query.TenantId, query.PropertyId).ToListAsync());

        }
    }
}
