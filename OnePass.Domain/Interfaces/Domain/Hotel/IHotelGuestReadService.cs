
namespace OnePass.Domain
{
    public interface IHotelGuestReadService
    {
        Task<HotelGuestResponse> GetHotelGuestAsync(GetHotelGuestByPhoneQuery query);

        Task<GuestSelfieResult> GetHotelGuestSelfieAsync(GetHotelGuestSelfieQuery query);

    }
}
