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
    public class GetPendingQrCodeMatchesByPhoneQueryHandler :
    QueryHandlerBase<
        GetPendingQrCodeMatchesByPhoneQuery,
        HotelPendingQrCodeMatchDetailedResponse>,
    IReadQueryHandler<
        GetPendingQrCodeMatchesByPhoneQuery,
        HotelPendingQrCodeMatchDetailedResponse>
    {
        private static readonly Func<
            OnePassDbContext,
            int,            // propertyId (0 = ignore)
            string,         // phoneCountryCode
            string,         // phoneNumber
            DateTimeOffset,
            IAsyncEnumerable<HotelPendingQrCodeMatchDetailedResponse>
        > GetPendingQrCodeMatchesByPhoneEntitiesCompiledQuery =
            EF.CompileAsyncQuery(
                (OnePassDbContext ctx,
                 int propertyId,
                 string phoneCountryCode,
                 string phoneNumber,
                 DateTimeOffset cutoffUtc) =>

                    ctx.HotelPendingQrCodeMatches
                       .AsNoTracking()
                       .Where(p =>
                           p.PhoneCountryCode == phoneCountryCode &&
                           p.PhoneNumber == phoneNumber &&
                           (propertyId == 0 || p.PropertyId == propertyId) &&
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

                           Status = p.Status,   // always Pending
                           CreatedAt = p.CreatedAt
                       })
            );

        public GetPendingQrCodeMatchesByPhoneQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetPendingQrCodeMatchesByPhoneQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<HotelPendingQrCodeMatchDetailedResponse>> HandleAllAsync()
        {
            throw new NotSupportedException(
                "Fetching all pending QR code matches by phone is not supported in this query.");
        }

        public async Task<IEnumerable<HotelPendingQrCodeMatchDetailedResponse>> HandleQueryAsync(
            GetPendingQrCodeMatchesByPhoneQuery query)
        {
            var cutoffUtc = DateTimeOffset.UtcNow.AddHours(-6);

            return await ExecuteQuerySafelyAsync(async ctx =>
                await GetPendingQrCodeMatchesByPhoneEntitiesCompiledQuery(
                    ctx,
                    query.PropertyId,
                    query.PhoneCountryCode,
                    query.PhoneNumber,
                    cutoffUtc
                ).ToListAsync());
        }
    }
}
