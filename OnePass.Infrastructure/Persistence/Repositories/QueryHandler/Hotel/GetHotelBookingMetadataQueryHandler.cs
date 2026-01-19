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
    public sealed class GetHotelBookingMetadataQueryHandler :
    QueryHandlerBase<GetHotelBookingMetadataQuery, HotelBookingMetadataResponse>,
    IReadQueryHandler<GetHotelBookingMetadataQuery, HotelBookingMetadataResponse>
    {
        private static readonly Func<
            OnePassDbContext,
            int,
            int,
            IAsyncEnumerable<HotelBookingMetadataResponse>
        > GetHotelBookingMetadataCompiledQuery =
            EF.CompileAsyncQuery(
                (OnePassDbContext ctx, int tenantId, int propertyId) =>

                    ctx.HotelBookingMetadata
                       .AsNoTracking()
                       .Where(b =>
                           b.TenantId == tenantId &&
                           b.PropertyId == propertyId)
                       .OrderByDescending(b => b.CreatedAt)
                       .Select(b => new HotelBookingMetadataResponse
                       {
                           TenantId = b.TenantId,
                           PropertyId = b.PropertyId,
                           BookingId = b.BookingId,

                           Ota = b.Ota,

                           PhoneCountryCode = b.PhoneCountryCode,
                           PhoneNumber = b.PhoneNumber,

                           AdultsCount = b.AdultsCount,
                           MinorsCount = b.MinorsCount,

                           WindowStart = b.WindowStart,
                           WindowEnd = b.WindowEnd,

                           CreatedAt = b.CreatedAt,

                           // LEFT JOIN on phone
                           PrimaryGuestFullName =
                               ctx.HotelGuests
                                  .Where(g =>
                                      g.PhoneCountryCode == b.PhoneCountryCode &&
                                      g.PhoneNumber == b.PhoneNumber)
                                  .Select(g => g.FullName)
                                  .FirstOrDefault()
                       })
            );

        public GetHotelBookingMetadataQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetHotelBookingMetadataQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public Task<IEnumerable<HotelBookingMetadataResponse>> HandleAllAsync()
        {
            throw new NotSupportedException(
                "Fetching all hotel booking metadata without tenant/property is not supported.");
        }

        public async Task<IEnumerable<HotelBookingMetadataResponse>> HandleQueryAsync(
            GetHotelBookingMetadataQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
                await GetHotelBookingMetadataCompiledQuery(
                        ctx,
                        query.TenantId,
                        query.PropertyId)
                    .ToListAsync());
        }
    }

}
