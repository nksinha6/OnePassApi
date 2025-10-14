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
    public class HotelGuestConfiguration : IEntityTypeConfiguration<HotelGuest>
    {
        public void Configure(EntityTypeBuilder<HotelGuest> builder)
        {
            builder.ToTable("hotel_guests");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("id")
                   .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.LinkedPrimaryGuestId)
                   .HasColumnName("linked_primary_guest_id");

            builder.Property(x => x.FullName)
                   .HasColumnName("full_name")
                   .HasMaxLength(150)
                   .IsRequired();

            builder.Property(x => x.DateOfBirth)
                   .HasColumnName("date_of_birth");

            builder.Property(x => x.Gender)
                   .HasColumnName("gender")
                   .HasMaxLength(20);

            builder.Property(x => x.PhoneCountryCode)
                   .HasColumnName("phone_country_code")
                   .HasMaxLength(10)
                   .IsRequired();

            builder.Property(x => x.PhoneNumber)
                   .HasColumnName("phone_number")
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(x => x.Email)
                   .HasColumnName("email")
                   .HasMaxLength(150);

            builder.Property(x => x.Nationality)
                   .HasColumnName("nationality")
                   .HasMaxLength(100);

            builder.Property(x => x.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("now()");

            builder.Property(x => x.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("now()");

            // Constraints
            builder.HasIndex(x => new { x.PhoneCountryCode, x.PhoneNumber })
                   .IsUnique()
                   .HasDatabaseName("uq_phone");

            builder.HasCheckConstraint("chk_phone_number", "phone_number ~ '^[0-9]{7,15}$'");

            builder.HasOne<HotelGuest>()
                   .WithMany()
                   .HasForeignKey(x => x.LinkedPrimaryGuestId)
                   .HasConstraintName("fk_primary_guest")
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
