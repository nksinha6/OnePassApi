using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class HotelUserRefreshToken
    {
        public string Token { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public int TenantId { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; } = false;
    }
}
