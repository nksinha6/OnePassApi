using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OnePass.Domain;

namespace OnePass.Infrastructure.Persistence
{
    public class InviteConfiguration : IEntityTypeConfiguration<Invite>
    {
        public void Configure(EntityTypeBuilder<Invite> builder)
        {
            builder.ToTable("invites");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.HostUserPhone)
                .IsRequired();

            builder.Property(i => i.UnitId)
                .IsRequired();

            builder.Property(i => i.Title)
                .IsRequired(false);

            builder.Property(i => i.Description)
                .IsRequired(false);

            builder.Property(i => i.StartTime)
                .IsRequired();

            builder.Property(i => i.Duration)
                .IsRequired();

            builder.Property(i => i.CheckinQrcode)
               .HasColumnName("checkin_qrcode")
                .IsRequired(false);

            builder.Property(i => i.CheckoutQrcode)
                .HasColumnName("checkout_qrcode")
                .IsRequired(false);

            builder.Property(i => i.VisitPurposeId)
                .IsRequired(false);

            builder.Property(i => i.Scope)   
               .HasMaxLength(200);

            builder.Property(i => i.ZoneLevelId); 
            builder.HasOne<ZoneLevel>()
                   .WithMany()
                   .HasForeignKey(i => i.ZoneLevelId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(i => i.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
