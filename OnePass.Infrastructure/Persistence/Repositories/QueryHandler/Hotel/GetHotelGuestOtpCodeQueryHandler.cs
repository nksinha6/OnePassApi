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
    public class GetHotelGuestOtpCodeQueryHandler :
    QueryHandlerBase<GetHotelGuestOtpCodeQuery, HotelGuestsOtpCode>,
    IReadQueryHandler<GetHotelGuestOtpCodeQuery, HotelGuestsOtpCode>
    {
        private static readonly Func<
            OnePassDbContext,
            string,
            string,
            Task<HotelGuestsOtpCode?>
        > _getOtpCodeQuery =
            EF.CompileAsyncQuery(
                (OnePassDbContext ctx, string phoneCountryCode, string phoneNumber) =>
                    ctx.HotelGuestsOtpCodes
                        .AsNoTracking()
                        .Where(o =>
                            o.PhoneCountryCode == phoneCountryCode
                            && o.PhoneNumber == phoneNumber)
                        .OrderByDescending(o => o.CreatedAt)
                        .FirstOrDefault()
            );

        public GetHotelGuestOtpCodeQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetHotelGuestOtpCodeQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<HotelGuestsOtpCode>> HandleQueryAsync(
            GetHotelGuestOtpCodeQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
            {
                var result = await _getOtpCodeQuery(
                    ctx,
                    query.PhoneCountryCode,
                    query.PhoneNumber
                );

                return result != null ?
                    new List<HotelGuestsOtpCode> { result } :
                    Enumerable.Empty<HotelGuestsOtpCode>();
            });
        }

        public Task<IEnumerable<HotelGuestsOtpCode>> HandleAllAsync()
        {
            throw new NotSupportedException(
                "Fetching all hotel guest OTP codes is not supported.");
        }

    }

}
