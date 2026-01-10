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
            TypeAdapterConfig<HotelGuestSelfieDto, HotelGuestSelfie>.NewConfig()
           .Map(dest => dest.PhoneCountryCode, src => src.PhoneCountryCode)
           .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
           .Map(dest => dest.ContentType,
                src => src.Selfie.ContentType ?? "image/jpeg")
           .Map(dest => dest.FileSize,
                src => src.Selfie.Length)
           .Map(dest => dest.CreatedAt,
                src => DateTimeOffset.UtcNow)

           // IMPORTANT: explicitly ignore ImageOid
           .Ignore(dest => dest.ImageOid)
           .Ignore(dest => dest.UpdatedAt);

        }    }
}
