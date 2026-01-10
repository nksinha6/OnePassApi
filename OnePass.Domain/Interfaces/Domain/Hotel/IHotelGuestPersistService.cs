using System.Linq.Expressions;

namespace OnePass.Domain
{
    public interface IHotelGuestPersistService
    {
        Task<HotelGuest> Persist(HotelGuest guest);

        Task<HotelGuestSelfie> PersistSelfieAsync(
    HotelGuestSelfie selfie,
    Stream selfieStream,
    CancellationToken ct = default);

        Task<HotelGuest> UpdateAadharData(UpdateAadharStatusParam param);
        Task<HotelGuestFaceCapture> PersistFaceCapture(HotelGuestFaceCapture hotelGuestFaceCapture);
    }
}
