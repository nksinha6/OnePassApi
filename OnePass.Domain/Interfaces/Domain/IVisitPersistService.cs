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
        Task<Visit> PersistVisitAsync(VisitDto request);
        Task<InviteGuest> UpdateRSVPStatus(UpdateRSVPParam param);
        Task<InviteGuest> UpdateNDAStatus(UpdateNDAParam param);
        Task<Visit> UpdateVisitNDAStatus(UpdateVisitNDAParam param);
        Task<Visit> UpdateVisitStatus(UpdateVisitStatusParam param);

        Task<Visit> CheckinVisit(Guid visitId);

        Task<Visit> CheckoutVisit(Guid visitId);
    }
}
