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
    public class HotelOwnerConfiguration : IEntityTypeConfiguration<HotelOwner>
    {
        public void Configure(EntityTypeBuilder<HotelOwner> builder)
        {
            builder.ToTable("hotel_owners");

            builder.HasKey(h => h.Id);

            builder.Property(h => h.Id);

            builder.Property(h => h.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(h => h.ContactEmail)
                .HasMaxLength(150);

            builder.Property(h => h.ContactPhone)
                .HasMaxLength(20);

            builder.Property(h => h.BillingAddress)
                .HasMaxLength(255);
        }
    }
}
