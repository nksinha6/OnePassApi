using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Dto
{
    public sealed class HotelUserPropertyItem
    {
        public int PropertyId { get; set; }
        public string PropertyName { get; set; } = null!;
    }

}
