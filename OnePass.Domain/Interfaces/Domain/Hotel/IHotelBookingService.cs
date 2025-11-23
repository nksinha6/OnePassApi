using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public interface IHotelBookingService
    {
        public Task<BookingCheckin> StartBookingCheckin(int tenantId, string bookingId);
        public Task<BookingCheckin> RecordBookingCheckin(int tenantId, string bookingId);
    }
}
