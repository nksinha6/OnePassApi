using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class PremiseResponse
    {
        public Guid Id { get; set; }

        public Guid TenantId { get; set; }  
        public string Name { get; set; } = null!;
        public short TypeId { get; set; }
        public string Type { get; set; } = null!;
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
