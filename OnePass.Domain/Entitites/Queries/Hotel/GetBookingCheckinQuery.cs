using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class GetBookingCheckinQuery : IReadQuery
    {
        public int TenantId { get; set; }
        public string BookingId { get; set; }
    }
}
