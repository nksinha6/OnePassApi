using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Dto
{
    public class PendingQrCodeMatchesByPhoneItemResponse
    {
        public int Id { get; set; }

        public int TenantId { get; set; }
        public int PropertyId { get; set; }

        public string BookingId { get; set; } = null!;

        public PendingQrStatus Status { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
