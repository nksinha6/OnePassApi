using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class HotelUser
    {
        public string Id { get; set; } = null!; // string like "abc.xyz"
        public int TenantId { get; set; }

        public string Role { get; set; } = "Admin";

    }
}
