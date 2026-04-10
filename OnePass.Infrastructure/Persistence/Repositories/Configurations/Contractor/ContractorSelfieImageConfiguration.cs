using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Infrastructure.Persistence
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using OnePass.Domain;

    public class ContractorSelfieImageConfiguration
        : IEntityTypeConfiguration<ContractorSelfieImage>
    {
        public void Configure(EntityTypeBuilder<ContractorSelfieImage> builder)
        {
            builder.ToTable("contractor_selfie_image");

            // Composite Primary Key
            builder.HasKey(x => new { x.PhoneCountryCode, x.PhoneNumber });

            builder.Property(x => x.PhoneCountryCode)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(x => x.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.Image)
                .IsRequired();

            builder.Property(x => x.ContentType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.FileSize)
                .IsRequired();
        }
    }
}
