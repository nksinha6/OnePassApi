using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Dto
{
    public class PremiseDto
    {
        public string Name { get; set; } = null!;
        public Guid TenantId { get; set; }  // FK to tenant
        public short TypeId { get; set; }
        public Guid? ParentId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Zip { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? GmapUrl { get; set; }
        public string? Admin { get; set; }
    }

}
