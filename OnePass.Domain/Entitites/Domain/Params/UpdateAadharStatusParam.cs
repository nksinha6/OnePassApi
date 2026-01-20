using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class UpdateAadharStatusParam
    {
        public string Uid { get; set; } = null!;
        public string PhoneCountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public string Nationality { get; set; } = null!;

        public SplitAddressDto? SplitAddress { get; set; }

    }

    public sealed class SplitAddressDto
    {
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? Dist { get; set; }
        public string? Subdist { get; set; }
        public string? Vtc { get; set; }
        public string? Po { get; set; }
        public string? Street { get; set; }
        public string? House { get; set; }
        public string? Landmark { get; set; }
        public string? Pincode { get; set; }
    }

}
