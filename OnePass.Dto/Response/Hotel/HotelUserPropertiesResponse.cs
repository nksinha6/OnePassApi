using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Dto
{
    public sealed class HotelUserPropertiesResponse
    {
        public string UserId { get; set; } = null!;
        public int TenantId { get; set; }

        public List<HotelUserPropertyItem> Properties { get; set; } = new();
    }

}
