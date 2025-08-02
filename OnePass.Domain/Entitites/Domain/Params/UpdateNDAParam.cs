using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class UpdateNDAParam
    {
        public Guid InviteId { get; set; }
        public string GuestId { get; set; }
    }
}
