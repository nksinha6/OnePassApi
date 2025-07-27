using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class Unit
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public Guid CompanyId { get; set; }    // FK → companies(id)
        public Guid? PropertyId { get; set; }  // FK → properties(id) (nullable)

        public int? Floor { get; set; }        // optional

        public string? AdminPhone { get; set; } // FK → users(phone) (nullable)
    }

}
