using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class GetInviteByHostPhoneQuery : IReadQuery
    {
        public string PhoneNo { get; set; }
    }
}
