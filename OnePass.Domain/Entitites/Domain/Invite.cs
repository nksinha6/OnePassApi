using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class Invite
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string HostUserPhone { get; set; } = null!;   // FK to users.phone
        public Guid UnitId { get; set; }                     // FK to units.id

        public string? Title { get; set; }
        public string? Description { get; set; }

        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }

        public string? CheckinQrcode { get; set; }
        public string? CheckoutQrcode { get; set; }

        public Guid? VisitPurposeId { get; set; }            
        public string? Scope { get; set; }   
        public Guid? ZoneLevelId { get; set; }  
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
