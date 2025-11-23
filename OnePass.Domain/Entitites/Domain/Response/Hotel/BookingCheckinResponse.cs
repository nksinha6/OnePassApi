using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class BookingCheckinResponse
    {
        public string BookingId { get; set; }
        public int TenantId { get; set; }

        public DateTimeOffset? ScheduledCheckinAt { get; set; }
        public DateTimeOffset? CheckinWindowStart { get; set; }

        public DateTimeOffset? ActualCheckinAt { get; set; }
        public DateTimeOffset? ActualCheckoutAt { get; set; }
    }

}
