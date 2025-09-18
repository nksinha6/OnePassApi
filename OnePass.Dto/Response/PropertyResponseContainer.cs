using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Dto.Response
{
    public class PropertyResponseContainer
    {
        public Guid CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public IEnumerable<PropertyItemResponse> Items { get; set; } = Enumerable.Empty<PropertyItemResponse>();

    }
}
