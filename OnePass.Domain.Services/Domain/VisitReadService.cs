using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnePass.Domain.Services;

namespace OnePass.Domain
{
    public class VisitReadService : ReadServiceBase, IVisitReadService
    {
        public VisitReadService(IReadRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }

        public Task<IEnumerable<VisitPurpose>> GetVisitPurposes(GetAllVisitPurposesQuery query) =>
        HandleQueryAsync<GetAllVisitPurposesQuery, VisitPurpose>(
                query,
                useStoredProcedure: false);

        public Task<IEnumerable<HostInviteDetail>> GetHostInvites(GetInviteByHostPhoneQuery query) =>
        HandleQueryAsync<GetInviteByHostPhoneQuery, HostInviteDetail>(
                query,
                useStoredProcedure: true);
    }
}
