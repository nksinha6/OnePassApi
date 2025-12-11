
namespace OnePass.Domain
{
    public interface IHotelGuestReadService
    {
        Task<HotelGuestResponse> GetHotelGuestAsync(GetHotelGuestByPhoneQuery query);
                Task<IEnumerable<HotelPendingFaceMatchResponse>> GetPendingFaceMatchesAsync(
    int tenantId,
    int propertyId);
    }
}
