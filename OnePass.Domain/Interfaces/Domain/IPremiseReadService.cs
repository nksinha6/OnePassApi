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
        Task<PremiseResponse> GetPremiseAsync(GetPremiseByIdQuery query);
        Task<IEnumerable<PremiseResponse>> GetPremisesAsync(GetPremisesByTenantIdQuery query);
        Task<IEnumerable<PremiseResponse>> GetPremisesAsync(GetPremisesByParentIdQuery query);
    }
}
