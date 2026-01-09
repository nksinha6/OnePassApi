using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class GetFaceMatchByBookingAndPhoneQuery : IReadQuery
    {
        public int TenantId { get; set; }
        public int PropertyId { get; set; }
        public string BookingId { get; set; }
        public string PhoneCountryCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
