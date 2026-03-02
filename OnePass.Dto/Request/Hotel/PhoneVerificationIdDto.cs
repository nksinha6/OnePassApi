using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Dto
{
    public class PhoneVerificationIdDto
    {
        public string VerificationId { get; set; }
        public long ReferenceId { get; set; }

        public string PhoneCountryCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
