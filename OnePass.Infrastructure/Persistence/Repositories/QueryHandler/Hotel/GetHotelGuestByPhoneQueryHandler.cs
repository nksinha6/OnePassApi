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
    public class GetHotelGuestByPhoneQueryHandler :
        QueryHandlerBase<GetHotelGuestByPhoneQuery, HotelGuestResponse>,
        IReadQueryHandler<GetHotelGuestByPhoneQuery, HotelGuestResponse>
    {
        private static readonly Func<OnePassDbContext, string, string, Task<HotelGuestResponse>> GetHotelGuestByPhoneCompiledQuery =
            EF.CompileAsyncQuery((OnePassDbContext ctx, string phoneCountryCode, string phoneNumber) =>
                (from g in ctx.HotelGuests.AsNoTracking()
                 where g.PhoneCountryCode == phoneCountryCode
                       && g.PhoneNumber == phoneNumber
                 select new HotelGuestResponse
                 {
                     Id = g.Id,
                     FullName = g.FullName,
                     PhoneCountryCode = g.PhoneCountryCode,
                     PhoneNumber = g.PhoneNumber,
                     Email = g.Email,
                     Nationality = g.Nationality,
                     Gender = g.Gender,
                     VerificationStatus = g.VerificationStatus,
                     DateOfBirth = g.DateOfBirth
                 })
                .FirstOrDefault());

        public GetHotelGuestByPhoneQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetHotelGuestByPhoneQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<HotelGuestResponse>> HandleAllAsync()
        {
            throw new NotSupportedException("Fetching all hotel guests is not supported in this query.");
        }

        public async Task<IEnumerable<HotelGuestResponse>> HandleQueryAsync(GetHotelGuestByPhoneQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
            {
                var result = await GetHotelGuestByPhoneCompiledQuery(ctx, query.PhoneCountryCode, query.PhoneNumber);
                return result != null ? new List<HotelGuestResponse> { result } : Enumerable.Empty<HotelGuestResponse>();
            });
        }
    }

}
