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
    public class CheckinGuestConfiguration : IEntityTypeConfiguration<CheckinGuest>
    {
        public void Configure(EntityTypeBuilder<CheckinGuest> builder)
        {
            builder.ToTable("checkin_guests");

            builder.HasKey(cg => new { cg.CheckinId, cg.GuestId })
                .HasName("pk_checkin_guests");

            builder.Property(cg => cg.CreatedAt)
                .HasDefaultValueSql("now()");

            builder.Property(cg => cg.UpdatedAt)
                .HasDefaultValueSql("now()");

            // Foreign key to guest_checkins
            builder.HasOne<GuestCheckin>()
                .WithMany()
                .HasForeignKey(cg => cg.CheckinId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_checkin");

            // Foreign key to hotel_guests
            builder.HasOne<HotelGuest>()
                .WithMany()
                .HasForeignKey(cg => cg.GuestId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_guest");
        }
    }
}
