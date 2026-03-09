using System.Linq.Expressions;
using OnePass.Dto;

namespace OnePass.Domain
{
    public interface IHotelGuestPersistService
    {
        Task<HotelGuest> Persist(HotelGuest guest);

        Task<HotelGuestAadhaarImage> PersistAadharImageAsync(
    HotelGuestAadhaarImage selfie,
    Stream selfieStream);

        Task<HotelGuest> UpdateAadharData(UpdateAadharStatusParam param);

        public Task<HotelGuest> UpdateEmailIdData(UpdateEmailIdParam param);
        Task<HotelGuestFaceCapture> PersistFaceCapture(HotelGuestFaceCapture hotelGuestFaceCapture);

        Task<HotelBookingGuest> PersistBookingGuestAsync(HotelBookingGuest hotelBookingGuest);

        public Task<bool> DeleteHotelGuest(DeleteGuestParam guest);

        Task<PhoneVerificationId> PersistPhoneVerificationIds(PhoneVerificationId phoneVerificationId);

        Task<HotelGuest> UpdateGuestStatusAsync(UpdateGuestStatusParam request);

        Task<HotelGuest> UpdateGuestProfileAsync(UpdateGuestProfile request);
    }
}
