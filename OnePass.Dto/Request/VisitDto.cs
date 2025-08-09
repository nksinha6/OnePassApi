using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Dto
{
    public class VisitDto
    {
        public string GuestPhone { get; set; }
        public string HostPhone { get; set; }
        public Guid VisitPurposeId { get; set; }
        public Guid? UnitId { get; set; }
        public int AccompanyingGuests { get; set; }
    }
}
