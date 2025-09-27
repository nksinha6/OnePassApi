using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public interface IHotelUserService
    {
       public Task SetPassword(string userId, string password, int tenantId);
    }
}
