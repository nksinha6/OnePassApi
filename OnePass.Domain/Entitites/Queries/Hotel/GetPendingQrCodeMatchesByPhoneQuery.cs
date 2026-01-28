using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class GetPendingQrCodeMatchesByPhoneQuery :IReadQuery
    {
        /// <summary>
        /// Optional. Pass 0 to ignore property filtering.
        /// </summary>
        public int PropertyId { get; init; }

        public string PhoneCountryCode { get; init; } = null!;

        public string PhoneNumber { get; init; } = null!;
    }

}
