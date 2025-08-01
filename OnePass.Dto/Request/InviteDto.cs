using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Dto
{
    public class InviteDto
    {
        public string HostUserPhone { get; set; } = null!;
        public Guid UnitId { get; set; }
        public string UnitName { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }

        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }

        public Guid? VisitPurposeId { get; set; }
        public string? Scope { get; set; }   
        public Guid? ZoneLevelId { get; set; }
        public List<string> GuestPhones { get; set; } = new();
    }
}
