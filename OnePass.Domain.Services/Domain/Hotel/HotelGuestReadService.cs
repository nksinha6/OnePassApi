using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain.Services
{
    public class HotelGuestReadService : ReadServiceBase, IHotelGuestReadService
    {
        public HotelGuestReadService(IReadRepositoryFactory repositoryFactory) : base(repositoryFactory) { }
        public Task<HotelGuestResponse> GetHotelGuestAsync(GetHotelGuestByPhoneQuery query) =>
             HandleSingleOrDefaultAsync<GetHotelGuestByPhoneQuery, HotelGuestResponse>(
                query,
                useStoredProcedure: false);

        public Task<IEnumerable<HotelPendingFaceMatchDetailedResponse>> GetPendingFaceMatchesAsync(int tenantId, int propertyId)
        =>
             HandleQueryAsync<GetPendingFaceMatchesQuery, HotelPendingFaceMatchDetailedResponse>(
                new GetPendingFaceMatchesQuery()
                { PropertyId = propertyId, TenantId = tenantId},
                useStoredProcedure: false);
    }
}
