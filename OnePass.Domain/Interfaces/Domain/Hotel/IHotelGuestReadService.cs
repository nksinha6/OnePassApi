
using OnePass.Dto;

namespace OnePass.Domain
{
    public interface IHotelGuestReadService
    {
        Task<HotelGuestResponse> GetHotelGuestAsync(GetHotelGuestByPhoneQuery query);

        Task<HotelGuestAadhaarImage> GetHotelGuestAadharImageAsync(GetHotelGuestAadharImageQuery query);

        Task<PendingQrCodeMatchesByPhoneResponse> GetPendingQrCodeMatchesByPhoneResponseAsync(GetPendingQrCodeMatchesByPhoneQuery query);
    }
}
