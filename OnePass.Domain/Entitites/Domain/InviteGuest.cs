using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class InviteGuest
    {
        public Guid InviteId { get; set; }        // FK to invites.id
        public string GuestPhone { get; set; } = null!;

        public string RsvpStatus { get; set; } = "pending";   // pending, accepted, declined

        public DateTime? CheckinTime { get; set; }
        public DateTime? CheckoutTime { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
