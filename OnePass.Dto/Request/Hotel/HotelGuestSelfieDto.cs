using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OnePass.Dto
{
    public class HotelGuestSelfieDto
    {
        public string PhoneCode { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public IFormFile Selfie { get; set; } = null!;
    }
}
