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

        public string Name { get; set; }

        public Guid CompanyId { get; set; }

        public Guid? PropertyId { get; set; }

        public int? Floor { get; set; }

        public string? AdminPhone { get; set; }
    }

}
