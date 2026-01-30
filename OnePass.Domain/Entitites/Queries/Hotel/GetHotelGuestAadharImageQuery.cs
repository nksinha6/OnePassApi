using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class GetHotelGuestAadharImageQuery : IReadQuery
    {
        public string PhoneCountryCode { get; set; }
        public string PhoneNumber { get; set; }
    }

}
