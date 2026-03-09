using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnePass.Dto;

namespace OnePass.Domain
{
    public class UpdateGuestStatusParam
    {
        public string PhoneCountryCode { get; set; }
        public string PhoneNumber { get; set; }

        public VerificationStatus VerificationStatus { get; set; }
    }
}
