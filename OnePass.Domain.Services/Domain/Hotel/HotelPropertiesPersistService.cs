using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain.Services
{
    public class HotelPropertiesPersistService(IPersistRepository<HotelTenant> hotelTenantRepository) : IHotelPropertiesPersistService
    {
        private readonly IPersistRepository<HotelTenant> _hotelTenantRepository = hotelTenantRepository;
        public Task<HotelTenant> PersistTenantAsync(HotelTenant request)
        => _hotelTenantRepository.AddOrUpdateAsync(request);
    }
}
