using OnePass.Domain.Services;
using OnePass.Domain;

namespace OnePass.API
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICampusPersistService, CampusPersistService>();
            services.AddScoped<ICampusReadService, CampusReadService>();
            return services;
        }
    }
}
