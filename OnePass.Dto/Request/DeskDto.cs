using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Dto
{
    public class DeskDto
    {
        public string Name { get; set; }
        public Guid UnitId { get; set; }
        public string? AdminPhone { get; set; }
        public string? AccessMode { get; set; }
        public string? AccessCategory { get; set; }
    }

}
