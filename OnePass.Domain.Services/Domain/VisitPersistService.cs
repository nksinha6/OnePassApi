using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using OnePass.Dto;

namespace OnePass.Domain.Services
{
    public class VisitPersistService(IPersistRepository<VisitPurpose> visitorPersistsRepository,
        IPersistRepository<Invite> inviteService,
        IPersistRepository<InviteGuest> inviteGuestService,
        IUserPersistsService userPersistService,
        IUnitOfWork unitOfWork) : IVisitPersistService
    {
        private readonly IPersistRepository<VisitPurpose> _visitorPersistsRepository = visitorPersistsRepository;

        private readonly IPersistRepository<Invite> _inviteService = inviteService;

        private readonly IPersistRepository<InviteGuest> _inviteGuestService = inviteGuestService;

        private readonly IUserPersistsService _userPersistService = userPersistService;

        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<VisitPurpose> PersistVisitPurposeAsync(VisitPurpose visitPurpose)
        {
            var result = await _visitorPersistsRepository.AddOrUpdateAllAsync(new List<VisitPurpose> { visitPurpose });
            return result.First();
        }

        public async Task<Invite> PersistInviteAsync(InviteDto request)
        {
            return await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                // 1️⃣ Map DTO to Invite entity
                var invite = request.Adapt<Invite>();
                invite.CheckinQrcode = QRCoder.GenerateQrCodeAsBase64($"Onepass.co.in/CHECKIN/{invite.Id}");
                invite.CheckoutQrcode = QRCoder.GenerateQrCodeAsBase64($"Onepass.co.in/CHECKOUT/{invite.Id}");

                // 2️⃣ Save invite (but changes won’t commit yet)
                var inviteResponse = await _inviteService.AddAsync(invite);

                // 3️⃣ Save guests for this invite
                foreach (var guest in request.GuestPhones)
                {
                    var inviteGuest = new InviteGuest
                    {
                        InviteId = invite.Id,
                        GuestPhone = guest
                    };

                    await _userPersistService.PersistsIfNotExistsAsync(guest);
                    await _inviteGuestService.AddAsync(inviteGuest);
                }

                // 4️⃣ Returning invite will trigger transaction commit in ExecuteInTransactionAsync
                return inviteResponse;
            });
        }

        public Task<InviteGuest> UpdateRSVPStatus(UpdateRSVPParam param) => _inviteGuestService.UpdatePartialAsync(new InviteGuest() { InviteId = param.InviteId, GuestPhone = param.GuestId, RsvpStatus = param.RSVPStatus }, x => x.RsvpStatus);
    }
}
