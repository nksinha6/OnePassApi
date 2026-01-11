using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class BookingVerificationWindow
    {
        public int TenantId { get; set; }
        public string BookingId { get; set; } = default!;
        public DateTimeOffset WindowStart { get; set; }
        public DateTimeOffset? WindowEnd { get; set; } 
    }
}
