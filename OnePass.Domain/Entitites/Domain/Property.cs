using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class Property
    {
        public Guid Id { get; set; }             // UUID (Primary Key)
        public Guid CompanyId { get; set; }      // Foreign Key to companies(id)

        public string Name { get; set; } = null!;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Pincode { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? GmapUrl { get; set; }

        public string AdminPhone { get; set; } = null!; // FK to users(phone)+----
    }
}
