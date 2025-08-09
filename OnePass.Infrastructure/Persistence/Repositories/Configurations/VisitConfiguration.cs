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
    public class VisitConfiguration : IEntityTypeConfiguration<Visit>
    {
        public void Configure(EntityTypeBuilder<Visit> builder)
        {
            builder.ToTable("visits");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Id)
                .IsRequired();

            builder.Property(v => v.GuestPhone)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(v => v.HostPhone)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(v => v.ApprovedByPhone)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(v => v.VisitPurposeId)
                .IsRequired();

            builder.Property(v => v.UnitId)
                .IsRequired(false);

            builder.Property(v => v.AccompanyingGuests)
                .HasDefaultValue(0);

            builder.Property(v => v.HasAcceptedNda)
                .HasDefaultValue(false);

            builder.Property(v => v.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("pending");

            builder.Property(v => v.CheckInTime)
                .IsRequired(false);

            builder.Property(v => v.CheckOutTime)
                .IsRequired(false);

            builder.Property(v => v.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(v => v.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
