using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class HotelTenantResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public byte[]? Logo { get; set; }

        public string? LogoContentType { get; set; }
    }
}
