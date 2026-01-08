using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class HotelGuestSelfie
    {
        public string PhoneCode { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public byte[] Selfie { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}
