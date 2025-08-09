using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class Visit
    {
        public Guid Id { get; set; }

        public string GuestPhone { get; set; }
        public string HostPhone { get; set; }
        public string ApprovedByPhone { get; set; }

        public Guid VisitPurposeId { get; set; }

        public Guid? UnitId { get; set; }
        public int AccompanyingGuests { get; set; }

        public bool HasAcceptedNda { get; set; }

        public string Status { get; set; }

        public DateTimeOffset? CheckInTime { get; set; }
        public DateTimeOffset? CheckOutTime { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
