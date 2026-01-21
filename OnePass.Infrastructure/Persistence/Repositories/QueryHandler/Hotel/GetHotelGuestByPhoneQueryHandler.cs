using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnePass.Domain;
using OpenTelemetry.Trace;

namespace OnePass.Infrastructure.Persistence
{
    public class GetHotelGuestByPhoneQueryHandler :
        QueryHandlerBase<GetHotelGuestByPhoneQuery, HotelGuestFlatResponse>,
        IReadQueryHandler<GetHotelGuestByPhoneQuery, HotelGuestFlatResponse>
    {
        private static readonly Func<OnePassDbContext, string, string, Task<HotelGuestFlatResponse>> GetHotelGuestByPhoneCompiledQuery =
            EF.CompileAsyncQuery((OnePassDbContext ctx, string phoneCountryCode, string phoneNumber) =>
                (from g in ctx.HotelGuests.AsNoTracking()
                 where g.PhoneCountryCode == phoneCountryCode
                       && g.PhoneNumber == phoneNumber
                 select new HotelGuestFlatResponse
                 {
                     Id = g.Id,
                     FullName = g.FullName,
                     PhoneCountryCode = g.PhoneCountryCode,
                     PhoneNumber = g.PhoneNumber,
                     Email = g.Email,
                     Nationality = g.Nationality,
                     Gender = g.Gender,
                     VerificationStatus = g.VerificationStatus,
                     DateOfBirth = g.DateOfBirth,
                     Uid = g.Uid,
                     SplitAddress = g.SplitAddress,
                 })
                .FirstOrDefault());

        public GetHotelGuestByPhoneQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetHotelGuestByPhoneQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<HotelGuestFlatResponse>> HandleAllAsync()
        {
            throw new NotSupportedException("Fetching all hotel guests is not supported in this query.");
        }

            public async Task<IEnumerable<HotelGuestFlatResponse>> HandleQueryAsync(GetHotelGuestByPhoneQuery query)
            {
                return await ExecuteQuerySafelyAsync(async ctx =>
                {
                    var result = await GetHotelGuestByPhoneCompiledQuery(ctx, query.PhoneCountryCode, query.PhoneNumber);
                    return result != null ? new List<HotelGuestFlatResponse> { result } : Enumerable.Empty<HotelGuestFlatResponse>();
                });
            }
    }

}
