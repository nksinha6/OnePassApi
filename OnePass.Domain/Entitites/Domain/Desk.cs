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
        public string Name { get; set; }
        public Guid UnitId { get; set; }
        public string? AdminPhone { get; set; }
        public string? AccessMode { get; set; }
        public string? AccessCategory { get; set; }
    }

}
