using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public sealed class HotelBookingMetadataResponse
    {
        public int TenantId { get; set; }
        public int PropertyId { get; set; }
        public string BookingId { get; set; }

        public string Ota { get; set; }

        public string PhoneCountryCode { get; set; }
        public string PhoneNumber { get; set; }

        public int AdultsCount { get; set; }
        public int MinorsCount { get; set; }

        public DateTimeOffset WindowStart { get; set; }
        public DateTimeOffset? WindowEnd { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string PrimaryGuestFullName { get; set; }
    }
}
