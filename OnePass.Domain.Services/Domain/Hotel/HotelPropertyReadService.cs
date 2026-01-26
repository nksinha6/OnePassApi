using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnePass.Domain.Services;
using OnePass.Dto;

namespace OnePass.Domain
{
    public class HotelPropertyReadService : ReadServiceBase, IHotelPropertyReadService
    {

        public HotelPropertyReadService(IReadRepositoryFactory repositoryFactory) : base(repositoryFactory) { }
        
        public Task<HotelPropertyNameResponse> GetPropertyNameAsync(int propertyId)
        => HandleSingleOrDefaultAsync<GetHotelPropertyNameByIdQuery, HotelPropertyNameResponse>(
               new GetHotelPropertyNameByIdQuery()
               {
                   PropertyId = propertyId
               },
               useStoredProcedure: false);
    }
}
