using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OnePass.Dto.Request.Hotel
{
    public class HotelTenantRequestDto
    {
        public string Name { get; set; } = null!;

        public IFormFile? Logo { get; set; }
    }
}
