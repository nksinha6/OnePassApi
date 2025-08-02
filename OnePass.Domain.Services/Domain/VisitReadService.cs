using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using OnePass.Domain.Entitites.Queries;
using OnePass.Domain.Services;
using OnePass.Dto;

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

        public async Task<IEnumerable<InviteResponseDto>> GetHostInvites(GetInviteByHostPhoneQuery query)
        {
            // 1️⃣ Get raw HostInviteDetail rows from stored proc
            var rawData = await HandleQueryAsync<GetInviteByHostPhoneQuery, HostInviteDetail>(
                query,
                useStoredProcedure: true);

            // 2️⃣ Adapt raw list → grouped InviteDto list using Mapster config
            return TransformInvites(rawData.ToList());
        }

        public async Task<IEnumerable<GuestInviteResponseDto>> GetGuestInvites(GetInvitesByGuestPhoneQuery query)
        {
            // 1️⃣ Get raw HostInviteDetail rows from stored proc
            var rawData = await HandleQueryAsync<GetInvitesByGuestPhoneQuery, GuestInviteDetail>(
                query,
                useStoredProcedure: true);

            // 2️⃣ Adapt raw list → grouped InviteDto list using Mapster config
            return TransformGuestInvites(rawData.ToList());
        }

        public List<InviteResponseDto> TransformInvites(List<HostInviteDetail> hostInviteDetails)
        {
            if (hostInviteDetails == null || !hostInviteDetails.Any())
                return new List<InviteResponseDto>();

            return hostInviteDetails
                .GroupBy(x => new
                {
                    x.InviteId,
                    x.Title,
                    x.Description,
                    x.UnitId,
                    x.UnitName,
                    x.StartTime,
                    x.Duration,
                    x.Scope,
                    x.VisitPurpose,
                    x.ZoneLevel,
                    x.HostPhone,
                    x.NdaRequired,
                })
                .Select(g => new InviteResponseDto
                {
                    InviteId = g.Key.InviteId,
                    Title = g.Key.Title,
                    Description = g.Key.Description,
                    UnitId = g.Key.UnitId,
                    UnitName = g.Key.UnitName,
                    StartTime = g.Key.StartTime,
                    Duration = g.Key.Duration,
                    Scope = g.Key.Scope,
                    VisitPurpose = g.Key.VisitPurpose,
                    ZoneLevel = g.Key.ZoneLevel,
                    HostPhone = g.Key.HostPhone,
                    NdaRequired = g.Key.NdaRequired,
                    // ✅ Guests list for this invite
                    Guests = g
                        .Where(x => !string.IsNullOrWhiteSpace(x.GuestPhone))
                        .Select(x => new GuestResponseDto
                        {
                            GuestPhone = x.GuestPhone,
                            GuestFirstName = x.GuestFirstName,
                            GuestLastName = x.GuestLastName,
                            GuestVerificationStatus = x.GuestVerificationStatus,
                            HasNdaAccepted = x.HasNdaAccepted
                        })
                        .GroupBy(guest => guest.GuestPhone)        // remove duplicates by phone
                        .Select(grp => grp.First())                // only first if multiple
                        .ToList()
                })
                .ToList();
        }

        public List<GuestInviteResponseDto> TransformGuestInvites(List<GuestInviteDetail> guestInviteDetails)
        {
            if (guestInviteDetails == null || !guestInviteDetails.Any())
                return new List<GuestInviteResponseDto>();

            return guestInviteDetails
                // ✅ First group by the guest (since you want one guest → many invites)
                .GroupBy(x => new
                {
                    x.GuestPhone,
                    x.GuestFirstName,
                    x.GuestLastName,
                    x.GuestVerificationStatus
                })
                .Select(g => new GuestInviteResponseDto
                {
                    // ✅ Fill the guest info once
                    guestDetails = new GuestResponseDto
                    {
                        GuestPhone = g.Key.GuestPhone,
                        GuestFirstName = g.Key.GuestFirstName,
                        GuestLastName = g.Key.GuestLastName,
                        GuestVerificationStatus = g.Key.GuestVerificationStatus
                    },

                    // ✅ Collect all invites for this guest
                    invites = g
                        .GroupBy(inv => new
                        {
                            inv.InviteId,
                            inv.Title,
                            inv.Description,
                            inv.UnitId,
                            inv.UnitName,
                            inv.StartTime,
                            inv.Duration,
                            inv.Scope,
                            inv.VisitPurpose,
                            inv.ZoneLevel,
                            inv.HostPhone
                        })
                        .Select(invGroup => new GuestInviteDetailDto
                        {
                            InviteId = invGroup.Key.InviteId,
                            Title = invGroup.Key.Title,
                            Description = invGroup.Key.Description,
                            UnitId = invGroup.Key.UnitId,
                            UnitName = invGroup.Key.UnitName,
                            StartTime = invGroup.Key.StartTime,
                            Duration = invGroup.Key.Duration,
                            Scope = invGroup.Key.Scope,
                            VisitPurpose = invGroup.Key.VisitPurpose,
                            ZoneLevel = invGroup.Key.ZoneLevel,
                            HostPhone = invGroup.Key.HostPhone
                        })
                        .ToList()
                })
                .ToList();
        }
    }
}
