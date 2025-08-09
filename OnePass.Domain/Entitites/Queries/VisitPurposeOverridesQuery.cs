using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class VisitPurposeOverridesQuery : IReadQuery
    {
        public Guid? CompanyId { get; set; }
        public Guid? UnitId { get; set; }
    }
}
