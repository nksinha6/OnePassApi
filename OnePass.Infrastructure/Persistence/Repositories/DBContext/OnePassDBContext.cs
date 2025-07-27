using Microsoft.EntityFrameworkCore;
using System.Reflection;
using OnePass.Domain;

namespace OnePass.Infrastructure.Persistence
{
    public class OnePassDbContext : DbContext
    {
        public OnePassDbContext(DbContextOptions<OnePassDbContext> options) : base(options) { }

        // Explicit DbSets for convenience
        public DbSet<Premise> Premises => Set<Premise>();
        public DbSet<Property> Properties => Set<Property>();
        public DbSet<Unit> Units => Set<Unit>();
        public DbSet<Desk> Desks => Set<Desk>();
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<User> Users => Set<User>();
        public DbSet<PremiseType> PremiseTypes => Set<PremiseType>();
        public DbSet<Tenant> Tenants => Set<Tenant>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply all IEntityTypeConfiguration<T> from this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Apply PostgreSQL-friendly lowercase naming convention
            modelBuilder.UseSnakeCaseNames();
        }
    }
}
