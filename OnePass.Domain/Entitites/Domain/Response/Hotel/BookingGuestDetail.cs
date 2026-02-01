using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class BookingGuestDetail
    {
        [Column("uid")]
        public string Uid { get; set; } = null!;

        [Column("full_name")]
        public string? FullName { get; set; }

        [Column("date_of_birth")]
        public DateTime? DateOfBirth { get; set; }

        [Column("gender")]
        public string? Gender { get; set; }

        [Column("phone_country_code")]
        public string PhoneCountryCode { get; set; } = null!;

        [Column("phone_number")]
        public string PhoneNumber { get; set; } = null!;

        [Column("email")]
        public string? Email { get; set; }

        [Column("nationality")]
        public string? Nationality { get; set; }

        [Column("reference_id")]
        public string? ReferenceId { get; set; }

        [Column("verification_id")]
        public string? VerificationId { get; set; }

        [Column("split_address")]
        public string? SplitAddress { get; set; }

        [Column("verification_status")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public VerificationStatusEnum? VerificationStatus { get; set; }

        [Column("booking_id")]
        public string BookingId { get; set; } = null!;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }

    public enum VerificationStatusEnum
    {
        pending,
        verified,
        rejected
        // match Postgres enum values
    }

}
