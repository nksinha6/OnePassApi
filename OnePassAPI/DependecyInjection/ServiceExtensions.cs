using OnePass.Domain.Services;
using OnePass.Domain;

namespace OnePass.API
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IPremisePersistService, PremisePersistService>();
            services.AddScoped<IPremiseReadService, PremiseReadService>();
            services.AddScoped<ICompanyReadService, CompanyReadService>();
            services.AddScoped<IUserReadService, UserReadService>();
            services.AddScoped<IUserPersistsService, UserPersistsService>();
            services.AddScoped<IVisitReadService, VisitReadService>();
            services.AddScoped<IVisitPersistService, VisitPersistService>();
            services.AddScoped<IHotelUserService, HotelUserService>();  
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IHotelGuestReadService, HotelGuestReadService>();
            
services.AddScoped<IHotelGuestPersistService, HotelGuestPersistService>();
            services.AddScoped<IHotelGuestAppService, HotelGuestAppService>();
            return services;
        }
    }
}
