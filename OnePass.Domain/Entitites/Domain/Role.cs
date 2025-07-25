using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class Role
    {
        public Guid RoleId { get; set; }

        public string Name { get; set; } = null!;   // e.g. "host", "admin"

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
