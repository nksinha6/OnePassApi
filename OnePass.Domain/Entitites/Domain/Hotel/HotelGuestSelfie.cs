using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class HotelGuestSelfie
    {
        public string PhoneCountryCode { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;

        // PostgreSQL OID is uint
        public uint ImageOid { get; set; }

        public string ContentType { get; set; } = default!;
        public long FileSize { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }

}
