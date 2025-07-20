using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class GetDesksByUnitIdQuery : IReadQuery
    {
        public Guid UnitId { get; set; }

    }
}
