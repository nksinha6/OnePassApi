using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnePass.Domain.Services;

namespace OnePass.Domain
{
    public class CompanyReadService : ReadServiceBase, ICompanyReadService
    {
        public CompanyReadService(IReadRepositoryFactory repositoryFactory) : base(repositoryFactory) { }

        public Task<IEnumerable<Company>> GetCompaniesAsync(GetAllCompaniesQuery query)
            =>
             HandleQueryAsync<GetAllCompaniesQuery, Company>(
                query,
                useStoredProcedure: false);
    }
}
