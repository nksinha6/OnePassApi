using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class HotelGuestBookingSelfie
    {
        public string BookingId { get; set; } = default!;

        public int TenantId { get; set; }      // NEW
        public int PropertyId { get; set; }

        public string PhoneCountryCode { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;

        public byte[] Image { get; set; } = default!;
        public string ContentType { get; set; } = default!;
        public long FileSize { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}
