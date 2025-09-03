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
    public class GuestFaceCaptureConfiguration : IEntityTypeConfiguration<GuestFaceCapture>
    {
        public void Configure(EntityTypeBuilder<GuestFaceCapture> builder)
        {
            builder.ToTable("guest_face_captures");

            builder.HasKey(f => f.FaceCaptureId);

            builder.Property(f => f.ImageUrl);

            builder.Property(f => f.FaceTemplate)
                .IsRequired();

            builder.Property(f => f.TemplateCreatedDatetime)
                .IsRequired();

            builder.Property(f => f.LiveCaptureDatetime)
                .IsRequired();

            builder.Property(f => f.FaceMatchScore)
                .HasPrecision(5, 2);

            builder.Property(f => f.MatchStatus)
                .HasConversion<string>()
                .HasDefaultValue(FaceMatchStatus.NotAttempted);

            builder.Property(f => f.VerificationMethod)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(f => f.CreatedAt)
                .HasDefaultValueSql("now()");

            builder.Property(f => f.UpdatedAt)
                .HasDefaultValueSql("now()");

            // Foreign key to hotel_guests
            builder.HasOne<HotelGuest>()
                .WithMany()
                .HasForeignKey(f => f.GuestId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_guest");

            // Foreign key to guest_checkins
            builder.HasOne<GuestCheckin>()
                .WithMany()
                .HasForeignKey(f => f.CheckinId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_checkin");
        }
    }
}
