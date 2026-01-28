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
    public class GetPendingQrCodeMatchesQueryHandler :
    QueryHandlerBase<GetPendingQrCodeMatchesQuery, HotelPendingQrCodeMatchDetailedResponse>,
    IReadQueryHandler<GetPendingQrCodeMatchesQuery, HotelPendingQrCodeMatchDetailedResponse>
    {
        private static readonly Func<
            OnePassDbContext,
            int,
            int,
            DateTimeOffset,
            IAsyncEnumerable<HotelPendingQrCodeMatchDetailedResponse>
        > GetPendingQrCodeMatchesEntitiesCompiledQuery =
            EF.CompileAsyncQuery(
                (OnePassDbContext ctx, int tenantId, int propertyId, DateTimeOffset cutoffUtc) =>

                    ctx.HotelPendingQrCodeMatches
                       .AsNoTracking()
                       .Where(p =>
                           p.TenantId == tenantId &&
                           p.PropertyId == propertyId &&
                           p.Status == PendingQrStatus.pending &&
                           p.CreatedAt >= cutoffUtc)
                       .OrderBy(p => p.CreatedAt)
                       .Select(p => new HotelPendingQrCodeMatchDetailedResponse
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

        public GetPendingQrCodeMatchesQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetPendingQrCodeMatchesQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<HotelPendingQrCodeMatchDetailedResponse>> HandleAllAsync()
        {
            throw new NotSupportedException("Fetching all pending QR code matches is not supported in this query.");
        }

        public async Task<IEnumerable<HotelPendingQrCodeMatchDetailedResponse>> HandleQueryAsync(
            GetPendingQrCodeMatchesQuery query)
        {
            var cutoffUtc = DateTimeOffset.UtcNow.AddHours(-6);

            return await ExecuteQuerySafelyAsync(async ctx =>
                await GetPendingQrCodeMatchesEntitiesCompiledQuery(
                    ctx,
                    query.TenantId,
                    query.PropertyId,
                    cutoffUtc
                ).ToListAsync());
        }
    }

}
