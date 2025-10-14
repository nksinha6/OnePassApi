using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class HotelUserResponse
    {
        public string Id { get; set; }
        public int TenantId { get; set; }
        public string TenantName { get; set; }
        public string AadharStatus { get; set; }
    }
}
