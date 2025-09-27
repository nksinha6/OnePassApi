using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public interface IRefreshTokenService
    {
        Task<HotelUserRefreshToken> CreateRefreshToken(string userId, int tenantId);
        Task<bool> ValidateRefreshToken(string token, string userId, int tenantId);
    }
}
