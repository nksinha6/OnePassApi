using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
