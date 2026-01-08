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
    public class GetHotelGuestSelfieQueryHandler :
    QueryHandlerBase<GetHotelGuestSelfieQuery, HotelGuestSelfie>,
    IReadQueryHandler<GetHotelGuestSelfieQuery, HotelGuestSelfie>
    {
        private static readonly Func<
            OnePassDbContext,
            string,
            string,
            IAsyncEnumerable<HotelGuestSelfie>
        > GetSelfieQuery =
            EF.CompileAsyncQuery(
                (OnePassDbContext ctx, string phoneCountryCode, string phoneNumber) =>
                    from hgs in ctx.HotelGuestSelfies.AsNoTracking()
                    where hgs.PhoneCode == phoneCountryCode
                          && hgs.PhoneNumber == phoneNumber
                    select new HotelGuestSelfie
                    {
                        PhoneCode = hgs.PhoneCode,
                        PhoneNumber = hgs.PhoneNumber,
                        Selfie = hgs.Selfie,
                        CreatedAt = hgs.CreatedAt,
                        UpdatedAt = hgs.UpdatedAt
                    });

        public GetHotelGuestSelfieQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetHotelGuestSelfieQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<HotelGuestSelfie>> HandleQueryAsync(
            GetHotelGuestSelfieQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
            {
                return await GetSelfieQuery(
                        ctx,
                        query.PhoneCountryCode,
                        query.PhoneNumber)
                    .ToListAsync();
            });
        }

        public Task<IEnumerable<HotelGuestSelfie>> HandleAllAsync()
        {
            throw new NotSupportedException(
                "Fetching all hotel guest selfies is not supported.");
        }
    }
}
