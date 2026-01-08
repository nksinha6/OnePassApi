using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class GetHotelGuestSelfieQuery
    {
        public string PhoneCountryCode { get; }
        public string PhoneNumber { get; }

        public GetHotelGuestSelfieQuery(string phoneCountryCode, string phoneNumber)
        {
            PhoneCountryCode = phoneCountryCode;
            PhoneNumber = phoneNumber;
        }
    }

}
