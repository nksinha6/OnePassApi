using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class UnitResponse
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public Guid? PropertyId { get; set; }
        public string? PropertyName { get; set; }
        public string Name { get; set; }
        public int? Floor { get; set; }
        public string? AdminPhone { get; set; }
    }

}
