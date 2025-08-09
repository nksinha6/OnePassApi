using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class VisitPurposeWithOverrides
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsVerificationRequired { get; set; }
        public bool IsHostApprovalRequired { get; set; }
    }
}
