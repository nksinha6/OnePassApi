using Mapster;
using Microsoft.Extensions.Configuration;
using OnePass.Dto;

namespace OnePass.Domain.Services
{
    public class HotelGuestReadService : ReadServiceBase, IHotelGuestReadService
    {
        public HotelGuestReadService(IConfiguration configuration, IReadRepositoryFactory repositoryFactory) : base(repositoryFactory) 
        {
        }
        public async Task<HotelGuestResponse> GetHotelGuestAsync(GetHotelGuestByPhoneQuery query)
        {
            var response = await HandleSingleOrDefaultAsync<GetHotelGuestByPhoneQuery, HotelGuestFlatResponse>(
               query,
               useStoredProcedure: false);

            return response.Adapt<HotelGuestResponse>();
        }

        public Task<HotelGuestAadhaarImage> GetHotelGuestAadharImageAsync(GetHotelGuestAadharImageQuery query)
        =>
            HandleSingleOrDefaultAsync<GetHotelGuestAadharImageQuery, HotelGuestAadhaarImage>(
                query,
                useStoredProcedure: false);

        public async Task<PendingQrCodeMatchesByPhoneResponse> GetPendingQrCodeMatchesByPhoneResponseAsync(GetPendingQrCodeMatchesByPhoneQuery query)
        {
            var response = await HandleQueryAsync<GetPendingQrCodeMatchesByPhoneQuery, HotelPendingQrCodeMatchDetailedResponse>(
                query,
                useStoredProcedure: false);
            return response.Adapt<PendingQrCodeMatchesByPhoneResponse>();
        }

        public Task<IEnumerable<BookingGuestDetail>> GetBookingGuestDetailAsync(BookingGuestQueryParameters query)
=> HandleQueryAsync<BookingGuestQueryParameters, BookingGuestDetail>(
    query,
    useStoredProcedure: true);

    }
}
