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

            builder.HasKey(g => g.Id);

            builder.Property(g => g.FullName)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(g => g.DateOfBirth);

            builder.Property(g => g.Gender)
                .HasMaxLength(20);

            builder.Property(g => g.PhoneCountryCode)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(g => g.PhoneNumber)
                .IsRequired();

            builder.Property(g => g.Email)
                .HasMaxLength(150);

            builder.Property(g => g.Nationality)
                .HasMaxLength(100);

            builder.Property(g => g.CreatedAt)
                .HasDefaultValueSql("now()");

            builder.Property(g => g.UpdatedAt)
                .HasDefaultValueSql("now()");

            // Self-referencing foreign key (no navigation property)
            builder.HasOne<HotelGuest>()
                .WithMany()
                .HasForeignKey(g => g.LinkedPrimaryGuestId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_primary_guest");
        }
    }
}
