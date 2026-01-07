using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class HotelPendingFaceMatchDetailedResponse
    {
        public long Id { get; set; }
        public string BookingId { get; set; } = default!;

        public string PhoneCountryCode { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public int TenantId { get; set; }
        public int PropertyId { get; set; }
        public string Status { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; set; }

        public string FullName { get; set; }
    }
}
