using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OnePass.Domain
{
    public interface IPremiseReadService
    {
        Task<IEnumerable<PropertyResponse>> GetPropertyAsync(GetPropertiesByCompanyIdQuery query);
        Task<IEnumerable<UnitResponse>> GetUnitsAsync(GetUnitsByCompanyIdQuery query);
        Task<IEnumerable<UnitResponse>> GetUnitsAsync(GetUnitsByPropertyIdQuery query);
        Task<IEnumerable<DeskResponse>> GetDesksAsync(GetDesksByCompanyIdQuery query);
        Task<IEnumerable<DeskResponse>> GetDesksAsync(GetDesksByUnitIdQuery query);
    }
}
