using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Dto.Response
{
    public class UnitResponseContainer
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public Guid? PropertyId { get; set; }
        public string? PropertyName { get; set; }
        public IEnumerable<UnitItemResponse> Items { get; set; } = Enumerable.Empty<UnitItemResponse>();

    }
}
