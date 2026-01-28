using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using OnePass.Dto;

namespace OnePass.Domain.Services
{
    public class HotelBookingReadService : ReadServiceBase,  IHotelBookingReadService
    {
        public HotelBookingReadService(IReadRepositoryFactory repositoryFactory) : base(repositoryFactory) { }

        public Task<IEnumerable<HotelPendingFaceMatchDetailedResponse>> GetPendingFaceMatchesAsync(int tenantId, int propertyId)
        =>
             HandleQueryAsync<GetPendingFaceMatchesQuery, HotelPendingFaceMatchDetailedResponse>(
                new GetPendingFaceMatchesQuery()
                { PropertyId = propertyId, TenantId = tenantId },
                useStoredProcedure: false);

        public Task<HotelPendingFaceMatchResponse> GetFaceMatchStatusAsync(GetFaceMatchByBookingAndPhoneQuery query )
        =>
             HandleSingleOrDefaultAsync<GetFaceMatchByBookingAndPhoneQuery, HotelPendingFaceMatchResponse>(query,
                useStoredProcedure: false);


public Task<IEnumerable<HotelBookingMetadataResponse>> GeHotelMetadataAsync(
GetHotelBookingMetadataQuery query)
        => HandleQueryAsync<GetHotelBookingMetadataQuery, HotelBookingMetadataResponse>(
            query,
                useStoredProcedure: false);

        public async Task<PendingQrCodeMatchesResponse> GetPendingQrCodeMatchesResponseAsync(GetPendingQrCodeMatchesQuery query)
        {
            var responses = await HandleQueryAsync<GetPendingQrCodeMatchesQuery, HotelPendingQrCodeMatchDetailedResponse>(
            query,
                useStoredProcedure: false);

            return responses.Adapt<PendingQrCodeMatchesResponse>();
        }
    }
}
