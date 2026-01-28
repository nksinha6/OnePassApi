using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Dto
{
    public class PendingQrCodeMatchesByPhoneResponse
    {
        public string PhoneCountryCode { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? FullName { get; set; }
        public IReadOnlyList<PendingQrCodeMatchesByPhoneItemResponse> Items { get; set; }
            = Array.Empty<PendingQrCodeMatchesByPhoneItemResponse>();
    }

}
