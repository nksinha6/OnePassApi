using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OnePass.Domain;
using OnePass.Dto;

namespace OnePass.Infrastructure.Persistence
{
    public class HotelGuestConfiguration : IEntityTypeConfiguration<HotelGuest>
    {
        public void Configure(EntityTypeBuilder<HotelGuest> builder)
        {
            builder.ToTable("hotel_guests");

            builder.HasKey(g => g.Id);

            builder.Property(g => g.Id)
                   .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(g => g.FullName)
                   .HasMaxLength(150);

            builder.Property(g => g.Gender)
                   .HasMaxLength(20);

            builder.Property(g => g.PhoneCountryCode)
                   .HasMaxLength(10)
                   .IsRequired();

            builder.Property(g => g.PhoneNumber)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(g => g.Email)
                   .HasMaxLength(150);
            builder.Property(x => x.Uid)
              .HasMaxLength(50);

            builder.Property(g => g.Nationality)
                   .HasMaxLength(100);

            builder.Property(x => x.SplitAddress)
                  .HasColumnType("jsonb");

            builder.Property(g => g.VerificationStatus)
                   .HasDefaultValue(VerificationStatus.pending);

            builder.Property(g => g.CreatedAt)
                   .HasDefaultValueSql("now()");

            builder.Property(g => g.UpdatedAt)
                   .HasDefaultValueSql("now()");

            // Unique constraint
            builder.HasIndex(g => new { g.PhoneCountryCode, g.PhoneNumber })
                   .IsUnique();

            // Check constraint
            builder.HasCheckConstraint(
                "chk_phone_number",
                "phone_number ~ '^[0-9]{7,15}$'"
            );
        }
    }
}
