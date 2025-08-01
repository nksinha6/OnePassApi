﻿using Microsoft.EntityFrameworkCore;
using System.Reflection;
using OnePass.Domain;

namespace OnePass.Infrastructure.Persistence
{
    public class OnePassDbContext : DbContext
    {
        public OnePassDbContext(DbContextOptions<OnePassDbContext> options) : base(options) { }

        // Explicit DbSets for convenience
        public DbSet<Property> Properties => Set<Property>();
        public DbSet<Unit> Units => Set<Unit>();
        public DbSet<Desk> Desks => Set<Desk>();
        public DbSet<AccessMode> AccessModes => Set<AccessMode>();
        public DbSet<AccessCategory> AccessCategories => Set<AccessCategory>();
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Tenant> Tenants => Set<Tenant>();
        public DbSet<VisitPurpose> VisitPurposes => Set<VisitPurpose>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply all IEntityTypeConfiguration<T> from this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Apply PostgreSQL-friendly lowercase naming convention
            modelBuilder.UseSnakeCaseNames();
        }
    }
}
