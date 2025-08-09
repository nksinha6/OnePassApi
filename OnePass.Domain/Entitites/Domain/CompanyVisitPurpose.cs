using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class CompanyVisitPurpose
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid VisitPurposeId { get; set; }
        public bool? IsVerificationRequired { get; set; }
        public bool? IsHostApprovalRequired { get; set; }
    }

    public class UnitVisitPurpose
    {
        public Guid Id { get; set; }
        public Guid UnitId { get; set; }
        public Guid VisitPurposeId { get; set; }
        public bool? IsVerificationRequired { get; set; }
        public bool? IsHostApprovalRequired { get; set; }
    }
}
