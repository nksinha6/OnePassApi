using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Dto
{
    public class PendingQrCodeMatchesResponse
    {
        public int TenantId { get; set; }
        public int PropertyId { get; set; }

        public IReadOnlyList<PendingQrCodeMatchItemResponse> Items { get; set; }
            = Array.Empty<PendingQrCodeMatchItemResponse>();
    }

}
