﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Dto
{
    public class HotelUserPasswordDto
    {
        public string UserId { get; set; } = null!;
        public int TenantId { get; set; }
        public string Password { get; set; } = null!;
    }
}
