using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnePass.Dto;

namespace OnePass.Domain
{
    public interface IHotelPropertyReadService
    {
        Task<HotelPropertyNameResponse> GetPropertyNameAsync(int propertyId);

    }
}
