
namespace OnePass.Domain
{
    public interface IHotelGuestReadService
    {
        Task<HotelGuestResponse> GetHotelGuestAsync(GetHotelGuestByPhoneQuery query);

        Task<HotelGuestSelfie> GetHotelGuestSelfieAsync(GetHotelGuestSelfieQuery query);

    }
}
