namespace OnePass.Domain.Services
{
    public class HotelBookingService(IPersistRepository<BookingCheckin> checkinRepository) : IHotelBookingService
    {
        IPersistRepository<BookingCheckin> _checkinRepository = checkinRepository;

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
    }
}
