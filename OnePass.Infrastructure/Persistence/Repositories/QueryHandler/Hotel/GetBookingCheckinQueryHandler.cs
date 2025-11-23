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
    public class GetBookingCheckinQueryHandler :
    QueryHandlerBase<GetBookingCheckinQuery, BookingCheckinResponse>,
    IReadQueryHandler<GetBookingCheckinQuery, BookingCheckinResponse>
    {
        private static readonly Func<OnePassDbContext, int, string, Task<BookingCheckinResponse>>
            GetBookingCheckinCompiledQuery =
            EF.CompileAsyncQuery((OnePassDbContext ctx, int tenantId, string bookingId) =>
                (from b in ctx.BookingCheckins.AsNoTracking()
                 where b.TenantId == tenantId
                       && b.BookingId == bookingId
                 select new BookingCheckinResponse
                 {
                     BookingId = b.BookingId,
                     TenantId = b.TenantId,

                     ScheduledCheckinAt = b.ScheduledCheckinAt,
                     CheckinWindowStart = b.CheckinWindowStart,

                     ActualCheckinAt = b.ActualCheckinAt,
                     ActualCheckoutAt = b.ActualCheckoutAt
                 })
                .FirstOrDefault());

        public GetBookingCheckinQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetBookingCheckinQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<BookingCheckinResponse>> HandleAllAsync()
        {
            throw new NotSupportedException("Fetching all booking checkins is not supported in this query.");
        }

        public async Task<IEnumerable<BookingCheckinResponse>> HandleQueryAsync(GetBookingCheckinQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
            {
                var result = await GetBookingCheckinCompiledQuery(ctx, query.TenantId, query.BookingId);
                return result != null ?
                    new List<BookingCheckinResponse> { result } :
                    Enumerable.Empty<BookingCheckinResponse>();
            });
        }
    }

}
