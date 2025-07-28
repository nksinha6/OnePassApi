using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnePass.Domain;
using OnePass.Infrastructure.Persistence;

namespace OnePass.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)

        {
            services.AddScoped(typeof(IPersistRepository<>), typeof(PersistRepository<>));
            services.AddScoped(typeof(IReadRepository<,>), typeof(EfReadRepository<,>));
            services.AddScoped(typeof(IStoredProcPersistRepository<>), typeof(StoredProcPersistRepository<>));
            services.AddScoped(typeof(IStoredProcReadRepository<,>), typeof(StoredProcReadRepository<,>));
            services.AddScoped<IReadRepositoryFactory, ReadRepositoryFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            //Add query handler
            services.AddScoped<IReadQueryHandler<GetPropertiesByCompanyIdQuery, PropertyResponse>, GetPropertiesByCompanyIdQueryHandler>();

            services.AddScoped<IReadQueryHandler<GetUnitsByCompanyIdQuery, UnitResponse>, GetUnitsByCompanyIdQueryHandler>();

            services.AddScoped<IReadQueryHandler<GetUnitsByPropertyIdQuery, UnitResponse>, GetUnitsByPropertyIdQueryHandler>();

            services.AddScoped<IReadQueryHandler<GetDesksByCompanyIdQuery, DeskResponse>, GetDesksByCompanyIdQueryHandler>();
            services.AddScoped<IReadQueryHandler<GetDesksByUnitIdQuery, DeskResponse>, GetDesksByUnitIdQueryHandler>();

services.AddScoped<IReadQueryHandler<GetUserQuery, User>, GetUserByPhoneQueryHandler>();

services.AddScoped<IReadQueryHandler<GetAllVisitPurposesQuery, VisitPurpose>, GetAllVisitPurposesQueryHandler>();
            services.AddScoped<IReadQueryHandler<GetAllCompaniesQuery, Company>, GetAllCompaniesQueryHandler>();
            // Add other infrastructure services here (e.g., DbContext, caching, etc.)
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<OnePassDbContext>(options =>
                options.UseNpgsql(connectionString));

            return services;
        }
    }
}
