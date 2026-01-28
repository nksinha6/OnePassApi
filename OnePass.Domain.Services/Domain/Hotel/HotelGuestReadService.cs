using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using Microsoft.Extensions.Configuration;
using Npgsql;
using OnePass.Dto;

namespace OnePass.Domain.Services
{
    public class HotelGuestReadService : ReadServiceBase, IHotelGuestReadService
    {
        public HotelGuestReadService(IConfiguration configuration, IReadRepositoryFactory repositoryFactory) : base(repositoryFactory) 
        {
        }
        public async Task<HotelGuestResponse> GetHotelGuestAsync(GetHotelGuestByPhoneQuery query)
        {
            var response = await HandleSingleOrDefaultAsync<GetHotelGuestByPhoneQuery, HotelGuestFlatResponse>(
               query,
               useStoredProcedure: false);

            return response.Adapt<HotelGuestResponse>();
        }

        public Task<HotelGuestSelfie> GetHotelGuestSelfieAsync(GetHotelGuestSelfieQuery query)
        =>
            HandleSingleOrDefaultAsync<GetHotelGuestSelfieQuery, HotelGuestSelfie>(
                query,
                useStoredProcedure: false);

        public async Task<PendingQrCodeMatchesByPhoneResponse> GetPendingQrCodeMatchesByPhoneResponseAsync(GetPendingQrCodeMatchesByPhoneQuery query)
        {
            var response = await HandleQueryAsync<GetPendingQrCodeMatchesByPhoneQuery, HotelPendingQrCodeMatchDetailedResponse>(
                query,
                useStoredProcedure: false);
            return response.Adapt<PendingQrCodeMatchesByPhoneResponse>();
        }

    }
}
