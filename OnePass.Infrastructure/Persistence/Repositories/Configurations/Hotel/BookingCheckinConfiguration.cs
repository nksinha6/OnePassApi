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
    public class BookingCheckinConfiguration : IEntityTypeConfiguration<BookingCheckin>
    {
        public void Configure(EntityTypeBuilder<BookingCheckin> builder)
        {
            builder.ToTable("booking_checkins");

            builder.HasKey(x => new { x.TenantId, x.BookingId });

            builder.Property(x => x.TenantId)
           .IsRequired();

            builder.Property(x => x.BookingId)
                .IsRequired();

            builder.Property(b => b.ScheduledCheckinAt);

            builder.Property(b => b.CheckinWindowStart)
                   .IsRequired();

            // actual checkin/checkout are optional -> no config needed

            builder.HasIndex(b => b.TenantId);

            builder.HasOne<HotelTenant>()
                   .WithMany()
                   .HasForeignKey(b => b.TenantId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
