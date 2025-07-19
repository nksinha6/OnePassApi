using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnePass.Domain.Services;

namespace OnePass.Domain
{
    public class PremiseReadService : ReadServiceBase, IPremiseReadService
    {
        public PremiseReadService(IReadRepositoryFactory repositoryFactory) : base(repositoryFactory) { }
        
        public Task<PremiseResponse> GetPremiseAsync(GetPremiseByIdQuery query) =>
            HandleSingleOrDefaultAsync<GetPremiseByIdQuery, PremiseResponse>(
                query,
                useStoredProcedure: false,
                notFoundMessage: "No Premise found for given Id");

        public Task<IEnumerable<PremiseResponse>> GetPremisesAsync(GetPremisesByTenantIdQuery query) =>
            HandleQueryAsync<GetPremisesByTenantIdQuery, PremiseResponse>(
                query,
                useStoredProcedure: false);

        public Task<IEnumerable<PremiseResponse>> GetPremisesAsync(GetPremisesByParentIdQuery query) =>
            HandleQueryAsync<GetPremisesByParentIdQuery, PremiseResponse>(
                query,
                useStoredProcedure: false);
    }
}
