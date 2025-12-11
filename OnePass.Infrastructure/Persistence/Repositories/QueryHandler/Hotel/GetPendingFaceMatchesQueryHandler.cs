using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnePass.Domain;
using OpenTelemetry.Trace;

namespace OnePass.Infrastructure.Persistence
{
    public class GetPendingFaceMatchesQueryHandler :
    QueryHandlerBase<GetPendingFaceMatchesQuery, HotelPendingFaceMatchResponse>,
    IReadQueryHandler<GetPendingFaceMatchesQuery, HotelPendingFaceMatchResponse>
    {
        // Compiled async query returning a List<HotelPendingFaceMatchResponse>
        private static readonly Func<OnePassDbContext, int, int, Task<List<HotelPendingFaceMatch>>>
    GetPendingFaceMatchesEntitiesCompiledQuery =
    EF.CompileAsyncQuery((OnePassDbContext ctx, int tenantId, int propertyId) =>
        ctx.HotelPendingFaceMatches
           .AsNoTracking()
           .Where(p => p.TenantId == tenantId &&
                       p.PropertyId == propertyId &&
                       p.Status == "pending")
           .OrderBy(p => p.CreatedAt)
           .ToList()
    );

        public GetPendingFaceMatchesQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetPendingFaceMatchesQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<HotelPendingFaceMatchResponse>> HandleAllAsync()
        {
            throw new NotSupportedException("Fetching all pending face matches is not supported in this query.");
        }

        public async Task<IEnumerable<HotelPendingFaceMatchResponse>> HandleQueryAsync(GetPendingFaceMatchesQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
            {
                var entities = await ctx.HotelPendingFaceMatches
            .AsNoTracking()
            .Where(p => p.TenantId == query.TenantId
                     && p.PropertyId == query.PropertyId
                     && p.Status == "pending")
            .OrderBy(p => p.CreatedAt)
            .ToListAsync(); // <-- normal EF call, translatable

        var results = entities.Select(p => new HotelPendingFaceMatchResponse
        {
            Id = p.Id,
            BookingId = p.BookingId,
            PhoneCountryCode = p.PhoneCountryCode,
            PhoneNumber = p.PhoneNumber,
            TenantId = p.TenantId,
            PropertyId = p.PropertyId,
            Status = p.Status,
            CreatedAt = p.CreatedAt
        }).ToList();

        return (IEnumerable<HotelPendingFaceMatchResponse>)results;
            });
        }
    }
}
