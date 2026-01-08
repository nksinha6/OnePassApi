using System.Linq.Expressions;

namespace OnePass.Domain
{
    public interface IHotelGuestPersistService
    {
        Task<HotelGuest> Persist(HotelGuest guest);

        Task<HotelGuestSelfie> PersistSelfie(HotelGuestSelfie selfieRequest);

        Task<HotelGuest> UpdateAadharData(UpdateAadharStatusParam param);
        Task<HotelGuestFaceCapture> PersistFaceCapture(HotelGuestFaceCapture hotelGuestFaceCapture);
    }
}
