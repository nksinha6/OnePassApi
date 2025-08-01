using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnePass.Dto;

namespace OnePass.Domain.Entitites.Queries
{
    public class GuestInviteResponseDto
    {
        
        public GuestResponseDto guestDetails { get; set; }

        public List<GuestInviteDetailDto> invites { get; set; } = new();
    }

    public class GuestInviteDetailDto
    {
        public Guid InviteId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public Guid UnitId { get; set; }
        public string UnitName { get; set; }

        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }

        public string Scope { get; set; }
        public string VisitPurpose { get; set; }
        public string ZoneLevel { get; set; }

        public string HostPhone { get; set; }
    }
}
