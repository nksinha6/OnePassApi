using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Dto
{
    public class DeskResponseContainer
    {
        public Guid UnitId { get; set; }
        public string UnitName { get; set; }

        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }

        public IEnumerable<DeskItemResponse> Items { get; set; } = Enumerable.Empty<DeskItemResponse>();
    }
}
