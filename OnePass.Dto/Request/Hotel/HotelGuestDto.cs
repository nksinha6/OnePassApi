using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Dto
{
    public class HotelGuestDto
    {
        public Guid? LinkedPrimaryGuestId { get; set; }
        public string FullName { get; set; } = default!;
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string PhoneCountryCode { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string? Email { get; set; }
        public string? Nationality { get; set; }

        public VerificationStatus VerificationStatus { get; set; } = VerificationStatus.Pending;
    }

    public enum VerificationStatus
    {
        Pending,
        Verified,
        Rejected,
        ManualReview
    }
}
