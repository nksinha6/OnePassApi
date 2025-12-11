using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OnePass.Domain;

namespace OnePass.Infrastructure.Persistence
{
    public class HotelPropertyConfiguration : IEntityTypeConfiguration<HotelProperty>
    {
        public void Configure(EntityTypeBuilder<HotelProperty> builder)
        {
            builder.ToTable("hotel_properties");

            // Primary key
            builder.HasKey(x => x.Id);

            // Auto-increment
            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();

            // Properties
            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(x => x.AddressLine1)
                   .HasMaxLength(255);

            builder.Property(x => x.AddressLine2)
                   .HasMaxLength(255);

            builder.Property(x => x.City)
                   .HasMaxLength(100);

            builder.Property(x => x.State)
                   .HasMaxLength(100);

            builder.Property(x => x.Country)
                   .HasMaxLength(100);

            builder.Property(x => x.Zipcode)
                   .HasMaxLength(20);

            builder.Property(x => x.ContactEmail)
                   .HasMaxLength(150);

            builder.Property(x => x.ContactPhone)
                   .HasMaxLength(20);

            // DateTimeOffset mapping to timestamptz in PostgreSQL
            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("now() AT TIME ZONE 'UTC'");

            builder.Property(x => x.UpdatedAt)
                   .HasDefaultValueSql("now() AT TIME ZONE 'UTC'");

            // Foreign keys without navigation properties
            builder.HasOne<HotelTenant>()
                   .WithMany()
                   .HasForeignKey(x => x.TenantId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_hotel_properties_tenant");

            builder.HasOne<LawEnforcementOffice>()
                   .WithMany()
                   .HasForeignKey(x => x.LawEnforcementId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("fk_hotel_properties_law_enforcement");
        }
    }
}
