using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class UpdateRSVPParam
    {
        public Guid InviteId { get; set; }
        public string GuestId { get; set; }
        public string RSVPStatus { get; set; }
    }
}
