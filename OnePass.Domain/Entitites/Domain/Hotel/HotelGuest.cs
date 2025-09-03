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
        public Guid? LinkedPrimaryGuestId { get; set; }
        public string FullName { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string PhoneCountryCode { get; set; } = null!;
        public byte[] PhoneNumber { get; set; } = null!;
        public string? Email { get; set; }
        public string? Nationality { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
