using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class Desk
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public Guid UnitId { get; set; }                 // FK → units(id)
        public string? AdminPhone { get; set; }          // FK → users(phone), nullable

        public Guid AccessModeId { get; set; }           // FK → access_modes(id)
        public Guid AccessCategoryId { get; set; }
    }

    public class AccessMode
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class AccessCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
