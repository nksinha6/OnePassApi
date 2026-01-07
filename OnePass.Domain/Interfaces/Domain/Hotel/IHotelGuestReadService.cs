
namespace OnePass.Domain
{
    public interface IHotelGuestReadService
    {
        Task<HotelGuestResponse> GetHotelGuestAsync(GetHotelGuestByPhoneQuery query);
                Task<IEnumerable<HotelPendingFaceMatchDetailedResponse>> GetPendingFaceMatchesAsync(
    int tenantId,
    int propertyId);
    }
}
