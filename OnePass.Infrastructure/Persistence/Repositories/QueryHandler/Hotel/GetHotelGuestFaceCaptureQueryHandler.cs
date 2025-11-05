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
    public class GetHotelGuestFaceCaptureQueryHandler :
    QueryHandlerBase<GetHotelGuestFaceCaptureQuery, HotelGuestFaceCaptureResponse>,
    IReadQueryHandler<GetHotelGuestFaceCaptureQuery, HotelGuestFaceCaptureResponse>
    {
        private static readonly Func<OnePassDbContext, int, string, string, Task<HotelGuestFaceCaptureResponse>>
            GetHotelGuestFaceCaptureCompiledQuery =
            EF.CompileAsyncQuery((OnePassDbContext ctx, int tenantId, string bookingId, string phoneNo) =>
                (from f in ctx.HotelGuestFaceCaptures.AsNoTracking()
                 where f.TenantId == tenantId
                    && f.BookingId == bookingId
                    && f.PhoneNo == phoneNo
                 select new HotelGuestFaceCaptureResponse
                 {
                     Id = f.Id,
                     TenantId = f.TenantId,
                     BookingId = f.BookingId,
                     PhoneNo = f.PhoneNo,
                     GuestId = f.GuestId,
                     LiveCaptureDatetime = f.LiveCaptureDatetime,
                     FaceMatchScore = f.FaceMatchScore,
                     CreatedAt = f.CreatedAt,
                     UpdatedAt = f.UpdatedAt
                 })
                .FirstOrDefault());

        public GetHotelGuestFaceCaptureQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetHotelGuestFaceCaptureQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<HotelGuestFaceCaptureResponse>> HandleAllAsync()
        {
            throw new NotSupportedException("Fetching all face captures is not supported in this query.");
        }

        public async Task<IEnumerable<HotelGuestFaceCaptureResponse>> HandleQueryAsync(GetHotelGuestFaceCaptureQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
            {
                var result = await GetHotelGuestFaceCaptureCompiledQuery(ctx, query.TenantId, query.BookingId, query.PhoneNo);
                return result != null ? new List<HotelGuestFaceCaptureResponse> { result } : Enumerable.Empty<HotelGuestFaceCaptureResponse>();
            });
        }
    }

}
