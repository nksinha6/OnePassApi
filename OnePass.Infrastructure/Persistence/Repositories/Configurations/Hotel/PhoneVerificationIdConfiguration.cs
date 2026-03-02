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

    public class PhoneVerificationIdConfiguration : IEntityTypeConfiguration<PhoneVerificationId>
    {
        public void Configure(EntityTypeBuilder<PhoneVerificationId> builder)
        {
            builder.ToTable("phone_verification_ids");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.VerificationId)
                .IsRequired();

            builder.Property(x => x.ReferenceId)
                .IsRequired();

            builder.Property(x => x.PhoneCountryCode)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(x => x.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW()");

            builder.HasIndex(x => new
            {
                x.PhoneCountryCode,
                x.PhoneNumber,
                x.VerificationId
            })
            .IsUnique()
            .HasDatabaseName("idx_phone_lookup");
        }
    }
}
