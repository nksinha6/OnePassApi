namespace OnePass.Domain.Services
{
    public class HotelBookingService(IPersistRepository<BookingCheckin> checkinRepository, IPersistRepository<HotelPendingFaceMatch> hotelPendingFaceMatchRepository) : IHotelBookingService
    {
        IPersistRepository<BookingCheckin> _checkinRepository = checkinRepository;
        IPersistRepository<HotelPendingFaceMatch> _hotelPendingFaceMatchRepository = hotelPendingFaceMatchRepository;

        public async Task<BookingCheckin> StartBookingCheckin(int tenantId, string bookingId)
        {
            var checkinBooking = new BookingCheckin()
            {
                TenantId = tenantId,
                BookingId = bookingId,
                CheckinWindowStart = DateTime.UtcNow
            };

            return await _checkinRepository.AddIfNotExistAsync(checkinBooking);
        }

        public Task<BookingCheckin> RecordBookingCheckin(int tenantId, string bookingId)
        =>
            _checkinRepository.UpdatePartialAsync(new BookingCheckin() { BookingId = bookingId, TenantId = tenantId, ActualCheckinAt = DateTime.UtcNow }, x => x.ActualCheckinAt);

        public Task<BookingCheckin> RecordBookingCheckout(int tenantId, string bookingId)
        =>
            _checkinRepository.UpdatePartialAsync(new BookingCheckin() { BookingId = bookingId, TenantId = tenantId, ActualCheckinAt = DateTime.UtcNow }, x => x.ActualCheckinAt);

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

        public Task<HotelPendingFaceMatch> VerifyBookingPendingFaceVerification(long id)
         =>
            _hotelPendingFaceMatchRepository.UpdatePartialAsync(new HotelPendingFaceMatch() { Id = id, Status = "verified" }, x => x.Status);
    }
}
