using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain.Services
{
    public class HotelGuestAppService(IHotelGuestReadService readService, IHotelGuestPersistService persistService) : IHotelGuestAppService
    {
        IHotelGuestReadService _readService = readService;
        IHotelGuestPersistService _persistService =
            persistService;

        public Task<HotelBookingGuest> AddBookingGyest(HotelBookingGuest hotelBookingGuest)
        => _persistService.PersistBookingGuestAsync(hotelBookingGuest);

        public async Task<HotelGuestResponse> GetForCreateIfNotExists(GetHotelGuestByPhoneQuery query)
        {
            var userResponse = await _readService.GetHotelGuestAsync(query);
            if (userResponse.Id == Guid.Empty)
            {
                  var guest = await _persistService.Persist(new HotelGuest() {       Id = Guid.NewGuid(),
                      PhoneCountryCode = query.PhoneCountryCode, PhoneNumber = query.PhoneNumber });

                return new HotelGuestResponse()
                {
                    Id = guest.Id,
                    PhoneCountryCode = guest.PhoneCountryCode,
                    PhoneNumber = guest.PhoneNumber
                };
            }

            return userResponse;
        }
    }
}
