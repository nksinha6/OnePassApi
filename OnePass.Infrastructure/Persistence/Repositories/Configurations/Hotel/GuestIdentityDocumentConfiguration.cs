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
    public class GuestIdentityDocumentConfiguration : IEntityTypeConfiguration<GuestIdentityDocument>
    {
        public void Configure(EntityTypeBuilder<GuestIdentityDocument> builder)
        {
            builder.ToTable("guest_identity_documents");

            builder.HasKey(d => d.DocumentId);

            builder.Property(d => d.DocumentType)
                .HasConversion<string>() // Map enum -> PostgreSQL enum
                .IsRequired();

            builder.Property(d => d.DocumentNumber)
                .IsRequired();

            builder.Property(d => d.IssuingCountry)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(d => d.ValidFrom);

            builder.Property(d => d.ValidUntil);

            builder.Property(d => d.DocumentImageUrl);

            builder.Property(d => d.OcrExtractedName)
                .HasMaxLength(150);

            builder.Property(d => d.OcrExtractedDob);

            builder.Property(d => d.FaceMatchScore)
                .HasPrecision(5, 2);

            builder.Property(d => d.VerificationStatus)
                .HasConversion<string>() // Map enum -> PostgreSQL enum
                .HasDefaultValue(VerificationStatus.pending);

            builder.Property(d => d.CreatedAt)
                .HasDefaultValueSql("now()");

            builder.Property(d => d.UpdatedAt)
                .HasDefaultValueSql("now()");

            // Foreign key (no navigation)
            builder.HasOne<HotelGuest>()
                .WithMany()
                .HasForeignKey(d => d.GuestId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_guest");
        }
    }
}
    