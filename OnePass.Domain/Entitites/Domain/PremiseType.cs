using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class PremiseType
    {
        public short Id { get; set; }   // SMALLINT in DB
        public string Name { get; set; } = null!;
    }
}
