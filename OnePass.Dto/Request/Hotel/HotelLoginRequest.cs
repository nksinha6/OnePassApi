using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class HotelLoginRequest
    {
        public string UserId { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
