using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace OnePass.Domain.Services
{
    public class HotelGuestReadService : ReadServiceBase, IHotelGuestReadService
    {
        private readonly string _connectionString;
        public HotelGuestReadService(IConfiguration configuration, IReadRepositoryFactory repositoryFactory) : base(repositoryFactory) 
        {
             _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public Task<HotelGuestResponse> GetHotelGuestAsync(GetHotelGuestByPhoneQuery query) =>
             HandleSingleOrDefaultAsync<GetHotelGuestByPhoneQuery, HotelGuestResponse>(
                query,
                useStoredProcedure: false);

        public Task<HotelGuestSelfie> GetHotelGuestSelfieAsync(GetHotelGuestSelfieQuery query)
        =>
            HandleSingleOrDefaultAsync<GetHotelGuestSelfieQuery, HotelGuestSelfie>(
                query,
                useStoredProcedure: false);
        
    }
}
