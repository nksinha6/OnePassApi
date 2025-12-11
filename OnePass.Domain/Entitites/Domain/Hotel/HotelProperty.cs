using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class HotelProperty
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string Name { get; set; } = default!;
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Zipcode { get; set; }
        public Guid? LawEnforcementId { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }

        // Use DateTimeOffset instead of DateTime
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
