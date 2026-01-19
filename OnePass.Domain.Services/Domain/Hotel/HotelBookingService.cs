namespace OnePass.Domain.Services
{
    public class HotelBookingService(IPersistRepository<HotelBookingMetadata> bookingMetadataRepository, IPersistRepository<HotelPendingFaceMatch> hotelPendingFaceMatchRepository, IPersistRepository<HotelGuestBookingSelfie> guestBookingSelfieRepository) : IHotelBookingService
    {
        IPersistRepository<HotelBookingMetadata> _bookingMetadataRepository = bookingMetadataRepository;
        IPersistRepository<HotelGuestBookingSelfie> _guestBookingSelfieRepository = guestBookingSelfieRepository;
        IPersistRepository<HotelPendingFaceMatch> _hotelPendingFaceMatchRepository = hotelPendingFaceMatchRepository;

        public Task<HotelBookingMetadata> StartBookingVerification(HotelBookingMetadata request)
        => _bookingMetadataRepository.AddIfNotExistAsync(request);
        
        public Task<HotelBookingMetadata> EndBookingVerification(int tenantId, int propertyId, string bookingId)
        =>            _bookingMetadataRepository.UpdatePartialAsync(new HotelBookingMetadata() { BookingId = bookingId, TenantId = tenantId, 
            PropertyId = propertyId,
WindowEnd = DateTime.UtcNow }, x => x.WindowEnd);

      
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
            await _guestBookingSelfieRepository.AddOrUpdateAsync( new HotelGuestBookingSelfie()
            {
                BookingId = bookingId,
                Image = bytes,
                ContentType = contentType,
                FileSize = length,
                PhoneCountryCode = phoneCountryCode,
                PhoneNumber = phoneNumber,
                Latitude = latitude,
                Longitude = longitude
            } );
                           return await _hotelPendingFaceMatchRepository.UpdatePartialAsync(new HotelPendingFaceMatch() { Id = id, Status = "verified"}, x => x.Status);
        }
    }
}
