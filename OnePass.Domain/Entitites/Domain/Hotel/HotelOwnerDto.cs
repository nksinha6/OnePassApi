using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain.Dto
{
    public class HotelOwnerDto
    {
        public string Name { get; set; } = null!;
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public string? BillingAddress { get; set; }
    }
}
