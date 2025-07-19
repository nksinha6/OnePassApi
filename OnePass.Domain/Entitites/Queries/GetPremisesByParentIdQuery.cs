using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class GetPremisesByParentIdQuery : IReadQuery
    {
        public Guid ParentId { get; set; }
    }
}
