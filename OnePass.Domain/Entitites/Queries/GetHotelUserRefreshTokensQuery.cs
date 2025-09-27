using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class GetHotelUserRefreshTokensQuery : IReadQuery
    {
        public string? Token { get; set; }
        public string? UserId { get; set; }
        public int? TenantId { get; set; }
    }
}
