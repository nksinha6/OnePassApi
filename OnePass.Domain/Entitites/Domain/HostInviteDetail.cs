using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class HostInviteDetail
    {
        public string HostPhone { get; set; }   // ✅ NEW FIELD

        public Guid InviteId { get; set; }
        public string InviteTitle { get; set; }
        public string InviteDescription { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }   // ✅ PostgreSQL INTERVAL maps well to TimeSpan
        public string Scope { get; set; }
        public string VisitPurpose { get; set; }
        public string ZoneLevel { get; set; }
        public string GuestPhone { get; set; }
        public string GuestFirstName { get; set; }
        public string GuestLastName { get; set; }
        public string GuestVerificationStatus { get; set; }
        public string RsvpStatus { get; set; }
    }
}
