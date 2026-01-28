using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class HotelPendingQrCodeMatchDetailedResponse
    {
        public int Id { get; set; }

        public int TenantId { get; set; }
        public int PropertyId { get; set; }

        public string BookingId { get; set; } = null!;

        public string PhoneCountryCode { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public string? FullName { get; set; }

        public PendingQrStatus Status { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }

}
