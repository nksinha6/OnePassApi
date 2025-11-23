using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
