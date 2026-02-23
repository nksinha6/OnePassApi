using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class GetHotelPropertyNameByIdQuery : IReadQuery
    {
        public int PropertyId { get; set; }
    }

    public class GetHotelTenantByIdQuery : IReadQuery
    {
        public int TenantId { get; set; }
    }

}
