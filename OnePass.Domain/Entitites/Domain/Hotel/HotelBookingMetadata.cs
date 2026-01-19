using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public sealed class HotelBookingMetadata
    {
        public int TenantId { get; set; }
        public int PropertyId { get; set; }
        public string BookingId { get; set; } = null!;

        public string Ota { get; set; } = null!;

        public string PhoneCountryCode { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public int AdultsCount { get; set; }
        public int MinorsCount { get; set; }

        public DateTimeOffset WindowStart { get; set; }
        public DateTimeOffset? WindowEnd { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }

}
