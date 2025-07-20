using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Dto
{
    public class UnitItemResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? Floor { get; set; }
        public string? AdminPhone { get; set; }
    }
}
