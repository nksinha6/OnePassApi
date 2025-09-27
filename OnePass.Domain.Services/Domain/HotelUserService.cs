using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain.Services
{
    public class HotelUserService(IPasswordHasher passwordHasher, IPersistRepository<HotelUserPassword> hotelUserPasswordService) : IHotelUserService
    {
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly IPersistRepository<HotelUserPassword> _hotelUserPasswordService = hotelUserPasswordService;

        public async Task SetPassword(string userId, string password, int tenantId)
        {
            var hashedPassword = _passwordHasher.HashPassword(password);
            await _hotelUserPasswordService.AddAsync(new HotelUserPassword { UserId = userId, PasswordHash = hashedPassword, TenantId = tenantId });
        }
    }
}
