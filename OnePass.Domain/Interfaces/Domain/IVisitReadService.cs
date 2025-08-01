using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnePass.Dto;

namespace OnePass.Domain
{
    public interface IVisitReadService
    {
        Task<IEnumerable<VisitPurpose>> GetVisitPurposes(GetAllVisitPurposesQuery query);

        Task<IEnumerable<InviteResponseDto>> GetHostInvites(GetInviteByHostPhoneQuery query);
    }
}
