using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnePass.Dto;

namespace OnePass.Domain
{
    public interface IHotelUserService
    {
       public Task SetPassword(string userId, string password, int tenantId);

        public Task<HotelUserPassword> GetPassword(string userId);
        public Task<HotelUserResponse> GetUser(string userId);

        public Task<HotelUserProperty> AddUserProperty(HotelUserProperty userProperty);
        Task<HotelUserPropertiesResponse> GetHotelUserProperties(string userId);
    }
}
