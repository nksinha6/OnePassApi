using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class ZoneLevel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;   
    }
}
