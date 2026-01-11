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
    public class GetBookingVerificationWindowQueryHandler :
    QueryHandlerBase<GetBookingVerificationWindowQuery, BookingVerificationWindow>,
    IReadQueryHandler<GetBookingVerificationWindowQuery, BookingVerificationWindow>
    {
        private static readonly Func<OnePassDbContext, int, string, Task<BookingVerificationWindow>>
            GetBookingCheckinCompiledQuery =
            EF.CompileAsyncQuery((OnePassDbContext ctx, int tenantId, string bookingId) =>
                (from b in ctx.BookingCheckins.AsNoTracking()
                 where b.TenantId == tenantId
                       && b.BookingId == bookingId
                 select new BookingVerificationWindow
                 {
                     BookingId = b.BookingId,
                     TenantId = b.TenantId,

                     WindowEnd = b.WindowEnd,
                     WindowStart = b.WindowStart
                 })
                .FirstOrDefault());

        public GetBookingVerificationWindowQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetBookingVerificationWindowQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<BookingVerificationWindow>> HandleAllAsync()
        {
            throw new NotSupportedException("Fetching all booking checkins is not supported in this query.");
        }

        public async Task<IEnumerable<BookingVerificationWindow>> HandleQueryAsync(GetBookingVerificationWindowQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
            {
                var result = await GetBookingCheckinCompiledQuery(ctx, query.TenantId, query.BookingId);
                return result != null ?
                    new List<BookingVerificationWindow> { result } :
                    Enumerable.Empty<BookingVerificationWindow>();
            });
        }
    }

}
