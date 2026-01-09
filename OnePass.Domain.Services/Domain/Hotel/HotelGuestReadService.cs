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

        public Task<HotelGuestSelfie> GetHotelGuestSelfieAsync(GetHotelGuestSelfieQuery query)
        =>   HandleSingleOrDefaultAsync<GetHotelGuestSelfieQuery, HotelGuestSelfie>(
                query,
                useStoredProcedure: false);

    }
}
