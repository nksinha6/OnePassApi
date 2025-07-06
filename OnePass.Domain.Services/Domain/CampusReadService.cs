using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnePass.Domain.Services;

namespace OnePass.Domain
{
    public class CampusReadService : ReadServiceBase, ICampusReadService
    {
        public CampusReadService(IReadRepositoryFactory repositoryFactory) : base(repositoryFactory) { }
        
        public Task<Campus> GetCampusAsync(GetCampusByIdQuery query) =>
            HandleSingleOrDefaultAsync<GetCampusByIdQuery, Campus>(
                query,
                useStoredProcedure: false,
                notFoundMessage: "No Campus found for given Id");
    }
}
