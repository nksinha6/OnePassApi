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
    public class GetHotelGuestAadhaarImageQueryHandler :
    QueryHandlerBase<GetHotelGuestAadharImageQuery, HotelGuestAadhaarImage>,
    IReadQueryHandler<GetHotelGuestAadharImageQuery, HotelGuestAadhaarImage>
    {
        private static readonly Func<
            OnePassDbContext,
            string,
            string,
            IAsyncEnumerable<HotelGuestAadhaarImage>
        > GetAadharImageQuery =
            EF.CompileAsyncQuery(
                (OnePassDbContext ctx, string phoneCountryCode, string phoneNumber) =>
                    from hgs in ctx.HotelGuestAadhaarImages.AsNoTracking()
                    where hgs.PhoneCountryCode == phoneCountryCode
                          && hgs.PhoneNumber == phoneNumber
                    select new HotelGuestAadhaarImage
                    {
                        PhoneCountryCode = hgs.PhoneCountryCode,
                        PhoneNumber = hgs.PhoneNumber,
                        Image = hgs.Image,
                        ContentType = hgs.ContentType,
                        FileSize = hgs.FileSize,
                        CreatedAt = hgs.CreatedAt,
                        UpdatedAt = hgs.UpdatedAt
                    });

        public GetHotelGuestAadhaarImageQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetHotelGuestAadhaarImageQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<HotelGuestAadhaarImage>> HandleQueryAsync(
            GetHotelGuestAadharImageQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
            {
                return await GetAadharImageQuery(
                        ctx,
                        query.PhoneCountryCode,
                        query.PhoneNumber)
                    .ToListAsync();
            });
        }

        public Task<IEnumerable<HotelGuestAadhaarImage>> HandleAllAsync()
        {
            throw new NotSupportedException(
                "Fetching all hotel guest selfies is not supported.");
        }
    }
}
