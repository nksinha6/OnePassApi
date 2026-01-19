using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OnePass.Dto
{
    public class HotelBookingGuestSelfieMatchRequestDto
    {
        public string BookingId { get; set; }
        public string PhoneCountryCode { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public long Id { get; set; }
        public IFormFile Selfie { get; set; } = null!;
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
    }
}
