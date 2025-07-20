using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Dto
{
    public class PropertyDto
    {
        public Guid CompanyId { get; set; }

        public string Name { get; set; }

        public string? Address { get; set; }
        public string? City { get; set; }

        public string? Pincode { get; set; }

        public string? State { get; set; }

        public string? Country { get; set; }
        public string? GmapUrl { get; set; }

        public string? AdminPhone { get; set; }
    }
}
