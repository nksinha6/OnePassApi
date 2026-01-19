using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class HotelPendingFaceMatch
    {
        public long Id { get; set; }

        public string BookingId { get; set; } = default!;
        public string PhoneCountryCode { get; set; }       // phone_country_code
        public string PhoneNumber { get; set; }            // phone_number

        public int TenantId { get; set; }
        public int PropertyId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string Status { get; set; } = "pending";
    }
}
