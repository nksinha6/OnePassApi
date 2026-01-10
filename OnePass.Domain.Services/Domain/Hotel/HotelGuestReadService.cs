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

        public async Task<GuestSelfieResult> GetHotelGuestSelfieAsync(GetHotelGuestSelfieQuery query)
        {
            

var result = await HandleSingleOrDefaultAsync<GetHotelGuestSelfieQuery, HotelGuestSelfie>(
                query,
                useStoredProcedure: false);
            if (result == null || result.ImageOid == null) return new();
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            var loManager = new NpgsqlLargeObjectManager(conn);
            var stream = loManager.OpenRead(result.ImageOid); // ⚠ DO NOT dispose here
            return new GuestSelfieResult
            {
                Stream = stream,
                ContentType = result.ContentType,
                FileSize = result.FileSize
            };
        }
    }
}
