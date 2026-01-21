using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; } = null!;      // The refresh token string
        public string UserId { get; set; } = null!;     // The ID of the user requesting refresh
        public int TenantId { get; set; }               // Tenant ID for multi-tenant system

        public List<int> PropertyIds { get; set; }
        public string Role { get; set; }
    }
}
