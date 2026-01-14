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
        public string PhoneCountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }

        public string Gender { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public string Nationality { get; set; } = null!;

    }
}
