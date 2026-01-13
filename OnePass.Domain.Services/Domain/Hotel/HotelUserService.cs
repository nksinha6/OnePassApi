using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain.Services
{
    public class HotelUserService : ReadServiceBase, IHotelUserService
    {
        private readonly IHasher _passwordHasher;
        private readonly IPersistRepository<HotelUserPassword> _hotelUserPasswordService;

        public HotelUserService(
        IHasher passwordHasher,
        IPersistRepository<HotelUserPassword> hotelUserPasswordRepository,
        IReadRepositoryFactory repositoryFactory
    ) : base(repositoryFactory)
        {
            _passwordHasher = passwordHasher;
            _hotelUserPasswordService = hotelUserPasswordRepository;
        }

        public async Task SetPassword(string userId, string password, int tenantId)
        {
            var hashedPassword = _passwordHasher.Hash(password);
            await _hotelUserPasswordService.AddAsync(new HotelUserPassword { UserId = userId, PasswordHash = hashedPassword, TenantId = tenantId });
        }

        public Task<HotelUserPassword> GetPassword(string userId, int tenantId) =>  HandleSingleOrDefaultAsync<GetHotelUserPasswordQuery, HotelUserPassword>(new GetHotelUserPasswordQuery() { UserId = userId, TenantId = tenantId },
            useStoredProcedure: false);

        public Task<HotelUserResponse> GetUser(string userId, int tenantId) => HandleSingleOrDefaultAsync<GetHotelUserByIdQuery, HotelUserResponse>(new GetHotelUserByIdQuery() { Id = userId, TenantId = tenantId },
            useStoredProcedure: false);
    }
}
