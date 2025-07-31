using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public interface IVisitReadService
    {
        Task<IEnumerable<VisitPurpose>> GetVisitPurposes(GetAllVisitPurposesQuery query);

        Task<IEnumerable<HostInviteDetail>> GetHostInvites(GetInviteByHostPhoneQuery query);
    }
}
