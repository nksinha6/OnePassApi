using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public sealed class HotelGuest
    {
        public Guid Id { get; set; }

        public string? Uid { get; set; } = null!;

        public string? FullName { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string PhoneCountryCode { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string? Email { get; set; }

        public string? Nationality { get; set; }

        // JSON stored as jsonb
        public string? SplitAddress { get; set; }

        public VerificationStatus VerificationStatus { get; set; } = VerificationStatus.pending;

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
