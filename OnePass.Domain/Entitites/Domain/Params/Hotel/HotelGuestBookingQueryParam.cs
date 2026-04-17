using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class HotelGuestBookingQueryParam : IReadQuery
    {
        public string p_phone_number { get; set; }
        public string p_country_code { get; set; }


    }
}
