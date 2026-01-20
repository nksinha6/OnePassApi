using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public sealed class HotelUserPropertyResponse
    {
        public string UserId { get; set; } = null!;
        public int TenantId { get; set; }
        public int PropertyId { get; set; }
        public string PropertyName { get; set; } = null!;
    }

}
