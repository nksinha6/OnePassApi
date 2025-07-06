using Mapster;
using OnePass.Domain;
using OnePass.Dto;

namespace OnePass.API
{
    public static class MapsterConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<CampusDto, Campus>.NewConfig()
                .IgnoreNullValues(true); // optional rule

            // Add more mappings here if needed
        }
    }
}
