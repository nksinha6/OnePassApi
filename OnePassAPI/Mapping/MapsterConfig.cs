using System.Text.Json;
using Mapster;
using OnePass.Domain;
using OnePass.Dto;
using OnePass.Dto.Response;

namespace OnePass.API
{
    public static class MapsterConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<CampusDto, Campus>.NewConfig()
                .IgnoreNullValues(true); // optional rule

                TypeAdapterConfig<IEnumerable<PropertyResponse>, PropertyResponseContainer>
                    .NewConfig()
                    .Map(dest => dest.CompanyId, src => src.FirstOrDefault().CompanyId)
                    .Map(dest => dest.CompanyName, src => src.FirstOrDefault().CompanyName)
                    .Map(dest => dest.Items, src => src.Select(x => x.Adapt<PropertyItemResponse>()));

            TypeAdapterConfig<IEnumerable<UnitResponse>, UnitResponseContainer>
            .NewConfig()
            .Map(dest => dest.CompanyId, src => src.FirstOrDefault().CompanyId)
            .Map(dest => dest.CompanyName, src => src.FirstOrDefault().CompanyName)
            .Map(dest => dest.PropertyId, src => src.FirstOrDefault().PropertyId)
            .Map(dest => dest.PropertyName, src => src.FirstOrDefault().PropertyName)
            .Map(dest => dest.Items, src => src.Select(x => x.Adapt<UnitItemResponse>()));

            TypeAdapterConfig<IEnumerable<DeskResponse>, DeskResponseContainer>
                .NewConfig()
                .Map(dest => dest.UnitId, src => src.FirstOrDefault().UnitId)
                .Map(dest => dest.UnitName, src => src.FirstOrDefault().UnitName)
                .Map(dest => dest.CompanyId, src => src.FirstOrDefault().CompanyId)
                .Map(dest => dest.CompanyName, src => src.FirstOrDefault().CompanyName)
                .Map(dest => dest.Items, src => src.Select(x => x.Adapt<DeskItemResponse>()));


            TypeAdapterConfig<InviteDto, Invite>
            .NewConfig()
           .Ignore(dest => dest.Id)
            .Ignore(dest => dest.CreatedAt)
            .Ignore(dest => dest.CheckinQrcode)
            .Ignore(dest => dest.CheckoutQrcode);
            // Add more mappings here if needed

            TypeAdapterConfig<VisitDto, Visit>
            .NewConfig()
            .Map(dest => dest.Id, src => Guid.NewGuid()) // auto-generate ID
            .Map(dest => dest.CreatedAt, src => DateTimeOffset.UtcNow)
            .Map(dest => dest.UpdatedAt, src => DateTimeOffset.UtcNow)
            .Map(dest => dest.Status, src => "pending") // default status
            .Ignore(dest => dest.CheckInTime)
            .Ignore(dest => dest.CheckOutTime)
            .Ignore(dest => dest.HasAcceptedNda);

            TypeAdapterConfig<HotelUserPasswordDto, HotelUserPassword>
             .NewConfig(); // prevent accidental overwrite

            TypeAdapterConfig<HotelGuestDto, HotelGuest>
          .NewConfig()
          .Map(dest => dest.Id, _ => Guid.NewGuid()) // generate new ID
          .Map(dest => dest.CreatedAt, _ => DateTime.UtcNow)
          .Map(dest => dest.UpdatedAt, _ => DateTime.UtcNow);

           TypeAdapterConfig<HotelGuestFaceCaptureDto, HotelGuestFaceCapture>.NewConfig()
           .Ignore(dest => dest.Id)
           .Ignore(dest => dest.CreatedAt)
           .Ignore(dest => dest.UpdatedAt)
           .Map(dest => dest.LiveCaptureDatetime, src => src.LiveCaptureDatetime == default
               ? DateTime.UtcNow
               : src.LiveCaptureDatetime);
            TypeAdapterConfig<HotelGuestSelfieDto, HotelGuestSelfie>
           .NewConfig()
           .Ignore(dest => dest.Image)        // handled separately
           .Ignore(dest => dest.CreatedAt)
           .Ignore(dest => dest.UpdatedAt)
           .Map(dest => dest.ContentType,
                src => string.IsNullOrWhiteSpace(src.Selfie.ContentType)
                    ? "application/octet-stream"
                    : src.Selfie.ContentType)
           .Map(dest => dest.FileSize,
                src => src.Selfie.Length);

            TypeAdapterConfig<HotelBookingMetadataDto, HotelBookingMetadata>
            .NewConfig()
            // PKs set by backend
            .Ignore(dest => dest.TenantId)
            .Ignore(dest => dest.PropertyId)

            // Verification window (set in mapping context)
            .Ignore(dest => dest.WindowStart)
            .Ignore(dest => dest.WindowEnd)

            // Audit fields (DB / backend owned)
            .Ignore(dest => dest.CreatedAt)
            .Ignore(dest => dest.UpdatedAt);

            TypeAdapterConfig
           .GlobalSettings
           .NewConfig<List<HotelUserPropertyResponse>, HotelUserPropertiesResponse>()
           .Map(dest => dest.UserId, src => src.First().UserId)
           .Map(dest => dest.TenantId, src => src.First().TenantId)
           .Map(dest => dest.Properties,
               src => src.Select(x => new HotelUserPropertyItem
               {
                   PropertyId = x.PropertyId,
                   PropertyName = x.PropertyName
               }).ToList());

            TypeAdapterConfig<HotelGuestFlatResponse, HotelGuestResponse>
           .NewConfig()
           .Map(dest => dest.SplitAddress,
                src => string.IsNullOrWhiteSpace(src.SplitAddress)
                    ? null
                    : JsonSerializer.Deserialize<SplitAddressDto>(
                          src.SplitAddress,
                           new JsonSerializerOptions()
                           {
                               PropertyNameCaseInsensitive = true
                           }));
            TypeAdapterConfig<PendingQRCodeMatchRequest, HotelPendingQrCodeMatch> 
           .NewConfig()
            .Map(dest => dest.BookingId, src => src.BookingId)
            .Map(dest => dest.PhoneCountryCode, src => src.PhoneCountryCode)
            .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
            .Map(dest => dest.CreatedAt, _ => DateTimeOffset.UtcNow)
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.TenantId)
            .Ignore(dest => dest.PropertyId)
            .Ignore(dest => dest.Status);

            // Single item mapping
            TypeAdapterConfig<HotelPendingQrCodeMatchDetailedResponse, PendingQrCodeMatchItemResponse>
                .NewConfig();

            // Collection → wrapper mapping
            TypeAdapterConfig<IEnumerable<HotelPendingQrCodeMatchDetailedResponse>, PendingQrCodeMatchesResponse>
                .NewConfig()
                .Map(dest => dest.TenantId,
                     src => src.Select(x => x.TenantId).FirstOrDefault())   // safe for empty
                .Map(dest => dest.PropertyId,
                     src => src.Select(x => x.PropertyId).FirstOrDefault()) // safe for empty
                .Map(dest => dest.Items,
                     src => src.Adapt<List<PendingQrCodeMatchItemResponse>>()); // safe for empty

            TypeAdapterConfig<HotelPendingQrCodeMatchDetailedResponse, PendingQrCodeMatchesByPhoneItemResponse>
     .NewConfig();

            // Collection → wrapper mapping
            TypeAdapterConfig<IEnumerable<HotelPendingQrCodeMatchDetailedResponse>, PendingQrCodeMatchesByPhoneResponse>
                .NewConfig()
                .Map(dest => dest.PhoneCountryCode,
                     src => src.Select(x => x.PhoneCountryCode).FirstOrDefault())
                .Map(dest => dest.PhoneNumber,
                     src => src.Select(x => x.PhoneNumber).FirstOrDefault())
                .Map(dest => dest.FullName,
                     src => src.Select(x => x.FullName).FirstOrDefault())
                .Map(dest => dest.Items,
                     src => src.Adapt<List<PendingQrCodeMatchesByPhoneItemResponse>>());


        }
    }
}
