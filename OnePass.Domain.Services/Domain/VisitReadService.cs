using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
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
                    x.HostPhone
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

                    // ✅ Guests list for this invite
                    Guests = g
                        .Where(x => !string.IsNullOrWhiteSpace(x.GuestPhone))
                        .Select(x => new GuestResponseDto
                        {
                            GuestPhone = x.GuestPhone,
                            GuestFirstName = x.GuestFirstName,
                            GuestLastName = x.GuestLastName,
                            GuestVerificationStatus = x.GuestVerificationStatus
                        })
                        .GroupBy(guest => guest.GuestPhone)        // remove duplicates by phone
                        .Select(grp => grp.First())                // only first if multiple
                        .ToList()
                })
                .ToList();
        }


    }
}
