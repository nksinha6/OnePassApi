using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnePass.Dto.Request.Hotel;

namespace OnePass.Domain
{
    public interface IHotelPropertiesPersistService
    {
        Task<HotelTenant> PersistTenantAsync(
    HotelTenant request);
    }
}
