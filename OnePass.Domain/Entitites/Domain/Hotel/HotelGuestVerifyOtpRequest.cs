using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class HotelGuestVerifyOtpRequest
    {
        public string PhoneCountryCode { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Otp { get; set; } = null!;
    }

}
