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
    public class HotelPropertyConfiguration : IEntityTypeConfiguration<HotelProperty>
    {
        public void Configure(EntityTypeBuilder<HotelProperty> builder)
        {
            builder.ToTable("hotel_properties");

            builder.HasKey(h => h.Id);

            builder.Property(h => h.Name)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(h => h.AddressLine1)
                .HasMaxLength(255);

            builder.Property(h => h.AddressLine2)
                .HasMaxLength(255);

            builder.Property(h => h.City)
                .HasMaxLength(100);

            builder.Property(h => h.State)
                .HasMaxLength(100);

            builder.Property(h => h.Country)
                .HasMaxLength(100);

            builder.Property(h => h.Zipcode)
                .HasMaxLength(20);

            builder.Property(h => h.ContactEmail)
                .HasMaxLength(150);

            builder.Property(h => h.ContactPhone)
                .HasMaxLength(20);

            builder.Property(h => h.CreatedAt)
                .HasDefaultValueSql("now()");

            builder.Property(h => h.UpdatedAt)
                .HasDefaultValueSql("now()");

            // Foreign keys (no navigation props)
            builder.HasOne<HotelOwner>() 
                .WithMany()
                .HasForeignKey(h => h.OwnerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_owner");

            builder.HasOne<LawEnforcementOffice>() 
                .WithMany()
                .HasForeignKey(h => h.LawEnforcementId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_law_enforcement");
        }
    }
}
