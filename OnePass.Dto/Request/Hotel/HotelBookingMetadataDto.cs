using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Dto
{
    public class HotelBookingMetadataDto
    {
        public string BookingId { get; set; } = null!;

        public string Ota { get; set; } = null!;

        public string PhoneCountryCode { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public int AdultsCount { get; set; }
        public int MinorsCount { get; set; }
    }
}
