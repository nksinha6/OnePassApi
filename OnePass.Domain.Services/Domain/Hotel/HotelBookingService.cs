namespace OnePass.Domain.Services
{
    public class HotelBookingService(IPersistRepository<BookingVerificationWindow> checkinRepository, IPersistRepository<HotelPendingFaceMatch> hotelPendingFaceMatchRepository, IPersistRepository<HotelGuestBookingSelfie> guestBookingSelfieRepository) : IHotelBookingService
    {
        IPersistRepository<BookingVerificationWindow> _checkinRepository = checkinRepository;
        IPersistRepository<HotelGuestBookingSelfie> _guestBookingSelfieRepository = guestBookingSelfieRepository;
        IPersistRepository<HotelPendingFaceMatch> _hotelPendingFaceMatchRepository = hotelPendingFaceMatchRepository;

        public async Task<BookingVerificationWindow> StartBookingVerification(int tenantId, string bookingId)
        {
            var checkinBooking = new BookingVerificationWindow()
            {
                TenantId = tenantId,
                BookingId = bookingId,
                WindowStart = DateTime.UtcNow
            };

            return await _checkinRepository.AddIfNotExistAsync(checkinBooking);
        }

        public Task<BookingVerificationWindow> EndBookingVerification(int tenantId, string bookingId)
        =>            _checkinRepository.UpdatePartialAsync(new BookingVerificationWindow() { BookingId = bookingId, TenantId = tenantId, WindowEnd = DateTime.UtcNow }, x => x.WindowEnd);

      
        public async Task<HotelPendingFaceMatch> RecordBookingPendingFaceVerification(int tenantId, int propertyId, FaceMatchInitiateRequest faceMatchInitiateRequest)
        {
            var hotelPendingFaceMatch = new HotelPendingFaceMatch()
            {
                TenantId = tenantId,
                BookingId = faceMatchInitiateRequest.BookingId,
                PropertyId = propertyId,
                PhoneCountryCode = faceMatchInitiateRequest.PhoneCountryCode,
                PhoneNumber = faceMatchInitiateRequest.PhoneNumber,
                Status = "pending",
                CreatedAt = DateTime.UtcNow
            };

            return await _hotelPendingFaceMatchRepository.AddIfNotExistAsync(hotelPendingFaceMatch);
        }

        public async Task<HotelPendingFaceMatch> VerifyBookingPendingFaceVerification(string bookingId, long id, byte[] bytes, string contentType, long length, string? latitude, string? longitude, string phoneCountryCode, string phoneNumber)
        {
            await _guestBookingSelfieRepository.AddOrUpdateAllAsync(new List<HotelGuestBookingSelfie>() { new HotelGuestBookingSelfie()
            {
                BookingId = bookingId,
                Image = bytes,
                ContentType = contentType,
                FileSize = length,
                PhoneCountryCode = phoneCountryCode,
                PhoneNumber = phoneNumber
            } });
                           return await _hotelPendingFaceMatchRepository.UpdatePartialAsync(new HotelPendingFaceMatch() { Id = id, Status = "verified"}, x => x.Status);
        }
    }
}
