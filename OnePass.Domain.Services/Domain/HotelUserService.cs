﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain.Services
{
    public class HotelUserService : ReadServiceBase, IHotelUserService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IPersistRepository<HotelUserPassword> _hotelUserPasswordService;

        public HotelUserService(
        IPasswordHasher passwordHasher,
        IPersistRepository<HotelUserPassword> hotelUserPasswordRepository,
        IReadRepositoryFactory repositoryFactory
    ) : base(repositoryFactory)
        {
            _passwordHasher = passwordHasher;
            _hotelUserPasswordService = hotelUserPasswordRepository;
        }

        public async Task SetPassword(string userId, string password, int tenantId)
        {
            var hashedPassword = _passwordHasher.HashPassword(password);
            await _hotelUserPasswordService.AddAsync(new HotelUserPassword { UserId = userId, PasswordHash = hashedPassword, TenantId = tenantId });
        }

        public Task<HotelUserPassword> GetPassword(string userId, int tenantId) =>  HandleSingleOrDefaultAsync<GetHotelUserPasswordQuery, HotelUserPassword>(new GetHotelUserPasswordQuery() { UserId = userId, TenantId = tenantId },
            useStoredProcedure: false);
        
    }
}
