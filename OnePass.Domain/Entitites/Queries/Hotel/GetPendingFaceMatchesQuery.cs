using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class GetPendingFaceMatchesQuery : IReadQuery
    { 
        public int TenantId { get; set; }
        public int PropertyId { get; set; }
    }
}
