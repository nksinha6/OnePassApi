using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class HotelGuestFlatResponse
    {
        public Guid Id { get; set; }

        public string? Uid { get; set; } = null!;

        public string FullName { get; set; } = default!;
        public string PhoneCountryCode { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string? Email { get; set; }
        public string? Nationality { get; set; }
        public string? Gender { get; set; }

        public string? SplitAddress { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public VerificationStatus VerificationStatus { get; set; } = VerificationStatus.pending;
        public DateOnly? DateOfBirth { get; set; }
    }
}
