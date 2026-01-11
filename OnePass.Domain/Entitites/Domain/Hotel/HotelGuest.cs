using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class HotelGuest
    {
        public Guid Id { get; set; }

        public string? FullName { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Gender { get; set; }

        public string PhoneCountryCode { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;

        public string? Email { get; set; }
        public string? Nationality { get; set; }

        public VerificationStatus VerificationStatus { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
    public enum VerificationStatus
    {
        pending,
        verified,
        rejected,
        manualReview
    }
}
