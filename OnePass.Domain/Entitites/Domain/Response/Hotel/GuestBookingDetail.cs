using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class GuestBookingDetail
    {
        [Column("booking_id")]
        public string BookingId { get; set; } = default!;

        [Column("tenant_id")]
        public int TenantId { get; set; }

        [Column("property_id")]
        public int PropertyId { get; set; }

        [Column("phone_country_code")]
        public string PhoneCountryCode { get; set; } = default!;

        [Column("phone_number")]
        public string PhoneNumber { get; set; } = default!;

        [Column("ota")]
        public string? Ota { get; set; }

        [Column("property_name")]
        public string? PropertyName { get; set; }

        [Column("tenant_name")]
        public string? TenantName { get; set; }
    }
}
