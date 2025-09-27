using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using OnePass.Domain.Services;

namespace OnePass.Domain
{
    public class RefreshTokenService : ReadServiceBase, IRefreshTokenService
    {
        private readonly IPersistRepository<HotelUserRefreshToken> _repo;
        private readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

        public RefreshTokenService(IPersistRepository<HotelUserRefreshToken> repo, IReadRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
            _repo = repo;
        }

        public async Task<HotelUserRefreshToken> CreateRefreshToken(string userId, int tenantId)
        {
            var tokenBytes = new byte[64];
            _rng.GetBytes(tokenBytes);
            var token = Convert.ToBase64String(tokenBytes);

            var refreshToken = new HotelUserRefreshToken
            {
                Token = token,
                UserId = userId,
                TenantId = tenantId,
                ExpiresAt = DateTime.UtcNow.AddDays(30)
            };

            await _repo.AddAsync(refreshToken);
            return refreshToken;
        }

        public async Task<bool> ValidateRefreshToken(string token, string userId, int tenantId)
        {
            var rt = await HandleSingleOrDefaultAsync<GetHotelUserRefreshTokensQuery, HotelUserRefreshToken>(new GetHotelUserRefreshTokensQuery() { Token = token, UserId = userId, TenantId = tenantId},
                useStoredProcedure: false);
            return rt != null && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow;
        }
    }
}
