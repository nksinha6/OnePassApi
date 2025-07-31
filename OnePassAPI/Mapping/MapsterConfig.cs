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
        }
    }
}
