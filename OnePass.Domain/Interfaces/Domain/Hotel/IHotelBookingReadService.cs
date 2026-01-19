using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public interface IHotelBookingReadService
    {
        Task<IEnumerable<HotelPendingFaceMatchDetailedResponse>> GetPendingFaceMatchesAsync(
int tenantId,
int propertyId);
Task<HotelPendingFaceMatchResponse> GetFaceMatchStatusAsync(GetFaceMatchByBookingAndPhoneQuery query);

        Task<IEnumerable<HotelBookingMetadataResponse>> GeHotelMetadataAsync(
GetHotelBookingMetadataQuery query);

    }
}
