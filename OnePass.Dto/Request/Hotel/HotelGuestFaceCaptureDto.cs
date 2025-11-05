using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Dto
{
    public class HotelGuestFaceCaptureDto
    {
        public int TenantId { get; set; }       // ✅ Newly added
        public string BookingId { get; set; } = null!;
        public string PhoneNo { get; set; } = null!;
        public string? GuestId { get; set; }
        public DateTime LiveCaptureDatetime { get; set; }
        public byte[]? FaceImage { get; set; }
        public decimal? FaceMatchScore { get; set; }

    }
}
