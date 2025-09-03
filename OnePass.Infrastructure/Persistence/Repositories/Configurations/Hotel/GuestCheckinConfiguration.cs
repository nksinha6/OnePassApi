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
    public class GuestCheckinConfiguration : IEntityTypeConfiguration<GuestCheckin>
    {
        public void Configure(EntityTypeBuilder<GuestCheckin> builder)
        {
            builder.ToTable("guest_checkins");

            builder.HasKey(c => c.CheckinId);

            builder.Property(c => c.CheckinDatetime)
                .IsRequired();

            builder.Property(c => c.CheckoutDatetime);

            builder.Property(c => c.VerificationMethod)
                .HasConversion<string>() 
                .IsRequired();

            builder.Property(c => c.VerificationStatus)
                .HasConversion<string>() 
                .HasDefaultValue(CheckinVerificationStatus.Yellow)
                .IsRequired();

            builder.Property(c => c.RoomNumber)
                .HasMaxLength(20);

            builder.Property(c => c.CreatedAt)
                .HasDefaultValueSql("now()");

            builder.Property(c => c.UpdatedAt)
                .HasDefaultValueSql("now()");

            // Foreign keys (no navigation properties)
            builder.HasOne<HotelProperty>()
                .WithMany()
                .HasForeignKey(c => c.PropertyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_property");

            builder.HasOne<HotelGuest>()
                .WithMany()
                .HasForeignKey(c => c.PrimaryGuestId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_primary_guest");
        }
    }
}
