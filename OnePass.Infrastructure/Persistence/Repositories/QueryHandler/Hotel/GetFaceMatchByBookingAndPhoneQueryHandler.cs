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
    public class GetFaceMatchByBookingAndPhoneQueryHandler :
    QueryHandlerBase<GetFaceMatchByBookingAndPhoneQuery, HotelPendingFaceMatchResponse>,
    IReadQueryHandler<GetFaceMatchByBookingAndPhoneQuery, HotelPendingFaceMatchResponse>
    {
        private static readonly Func<
            OnePassDbContext,
            string,
            string,
            string,
            int,
             int, Task<HotelPendingFaceMatchResponse>
        > GetFaceMatchCompiledQuery =
            EF.CompileAsyncQuery(
                (OnePassDbContext ctx,
                 string bookingId,
                 string phoneCountryCode,
                 string phoneNumber,
                 int tenantId,
                 int propertyId) =>

                    ctx.HotelPendingFaceMatches
                       .AsNoTracking()
                       .Where(p =>
                           p.BookingId == bookingId &&
                           p.PhoneCountryCode == phoneCountryCode &&
                           p.PhoneNumber == phoneNumber &&
                       p.TenantId == tenantId && 
                       p.PropertyId == propertyId)
                       .OrderBy(p => p.CreatedAt)
                       .Select(p => new HotelPendingFaceMatchResponse
                       {
                           Id = p.Id,
                           BookingId = p.BookingId,

                           PhoneCountryCode = p.PhoneCountryCode,
                           PhoneNumber = p.PhoneNumber,

                           TenantId = p.TenantId,
                           PropertyId = p.PropertyId,

                           Status = p.Status,
                           CreatedAt = p.CreatedAt
                       })
                       .FirstOrDefault()
            );

        public GetFaceMatchByBookingAndPhoneQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetFaceMatchByBookingAndPhoneQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<HotelPendingFaceMatchResponse>> HandleAllAsync()
        {
            throw new NotSupportedException("Fetching all face matches is not supported in this query.");
        }

        public async Task<IEnumerable<HotelPendingFaceMatchResponse>> HandleQueryAsync(
            GetFaceMatchByBookingAndPhoneQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
            {
                var result = await GetFaceMatchCompiledQuery(
                    ctx,
                    query.BookingId,
                    query.PhoneCountryCode,
                    query.PhoneNumber,
                    query.TenantId,
                    query.PropertyId);

                return result != null
                    ? new List<HotelPendingFaceMatchResponse> { result }
                    : Enumerable.Empty<HotelPendingFaceMatchResponse>();
            });
        }
    }

}
