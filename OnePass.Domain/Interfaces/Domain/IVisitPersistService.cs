using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnePass.Dto;

namespace OnePass.Domain
{
    public interface IVisitPersistService
    {
        Task<VisitPurpose> PersistVisitPurposeAsync(VisitPurpose request);
        Task<Invite> PersistInviteAsync(InviteDto request);
        Task<InviteGuest> UpdateRSVPStatus(UpdateRSVPParam param);
    }
}
