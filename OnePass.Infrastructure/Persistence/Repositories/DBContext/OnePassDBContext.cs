using Microsoft.EntityFrameworkCore;
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
        public DbSet<VisitPurpose> VisitPurposes => Set<VisitPurpose>();

        public DbSet<HotelTenant> HotelTenants => Set<HotelTenant>();
        public DbSet<HotelUser> HotelUsers => Set<HotelUser>();
        public DbSet<HotelPendingFaceMatch> HotelPendingFaceMatches => Set<HotelPendingFaceMatch>();

        public DbSet<HotelGuest> HotelGuests => Set<HotelGuest>();
        public DbSet<HotelGuestFaceCapture> HotelGuestFaceCaptures => Set<HotelGuestFaceCapture>()

            public DbSet<HotelGuestSelfie> HotelGuestSelfies => Set<HotelGuestSelfie>();

        public DbSet<HotelUserPassword> HotelUserPasswords => Set<HotelUserPassword>();

        public DbSet<HotelUserRefreshToken> HotelUserRefreshToken => Set<HotelUserRefreshToken>();

        public DbSet<BookingCheckin> BookingCheckins => Set<BookingCheckin>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
            /// Register PostgreSQL enum type first
            modelBuilder.HasPostgresEnum<VerificationStatus>("verification_status_enum");

            // Apply PostgreSQL-friendly naming convention
            modelBuilder.UseSnakeCaseNames();


            // Apply all IEntityTypeConfiguration<T> from this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            }
    }
}
