using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnePass.Dto
{
    public class PendingQrCodeMatchItemResponse
    {
        public int Id { get; set; }

        public string BookingId { get; set; } = null!;

        public string PhoneCountryCode { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public string? FullName { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PendingQrStatus Status { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }

    public enum PendingQrStatus
    {
        pending,
        verified,
        error
    }

}
