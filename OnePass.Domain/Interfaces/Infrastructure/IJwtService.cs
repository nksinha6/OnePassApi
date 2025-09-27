using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public interface IJwtService
    {
        string GenerateToken(string userId, int tenantId, string role);
    }
}
