using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public interface IHotelGuestAppService
    {
        Task<HotelGuestResponse> GetForCreateIfNotExists(GetHotelGuestByPhoneQuery query);

        Task<HotelBookingGuest> AddBookingGyest(HotelBookingGuest hotelBookingGuest);
    }
}
