﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OnePass.Domain;
using OnePass.Infrastructure.Persistence;
using OnePass.Infrastructure.Security;

namespace OnePass.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)

        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<OnePassDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddSingleton(_ =>
    new NpgsqlConnection(connectionString));

            services.AddScoped(typeof(IPersistRepository<>), typeof(PersistRepository<>));
            services.AddScoped(typeof(IReadRepository<,>), typeof(EfReadRepository<,>));
            services.AddScoped(typeof(IStoredProcPersistRepository<>), typeof(StoredProcPersistRepository<>));
            services.AddScoped(typeof(IStoredProcReadRepository<,>), typeof(StoredProcReadRepository<,>));
            services.AddScoped<IReadRepositoryFactory, ReadRepositoryFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPasswordHasher, Sha256PasswordHasher>();
            services.AddScoped<IJwtService, JwtService>();


            //Add query handler
            services.AddScoped<IReadQueryHandler<GetPropertiesByCompanyIdQuery, PropertyResponse>, GetPropertiesByCompanyIdQueryHandler>();

            services.AddScoped<IReadQueryHandler<GetUnitsByCompanyIdQuery, UnitResponse>, GetUnitsByCompanyIdQueryHandler>();

            services.AddScoped<IReadQueryHandler<GetUnitsByPropertyIdQuery, UnitResponse>, GetUnitsByPropertyIdQueryHandler>();

            services.AddScoped<IReadQueryHandler<GetDesksByCompanyIdQuery, DeskResponse>, GetDesksByCompanyIdQueryHandler>();
            services.AddScoped<IReadQueryHandler<GetDesksByUnitIdQuery, DeskResponse>, GetDesksByUnitIdQueryHandler>();

services.AddScoped<IReadQueryHandler<GetUserQuery, User>, GetUserByPhoneQueryHandler>();

services.AddScoped<IReadQueryHandler<GetAllVisitPurposesQuery, VisitPurpose>, GetAllVisitPurposesQueryHandler>();
            services.AddScoped<IReadQueryHandler<GetAllCompaniesQuery, Company>, GetAllCompaniesQueryHandler>();

            services.AddScoped<IReadQueryHandler<GetHotelUserPasswordQuery, HotelUserPassword>, GetHotelUserPasswordByIdQueryHandler>();

            services.AddScoped<IReadQueryHandler<GetHotelUserRefreshTokensQuery, HotelUserRefreshToken>, GetHotelUserRefreshTokensQueryHandler>();
            // Add other infrastructure services here (e.g., DbContext, caching, etc.)

            return services;
        }
    }
}
