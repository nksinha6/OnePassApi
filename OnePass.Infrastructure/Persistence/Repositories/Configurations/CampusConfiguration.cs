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
    public class CampusConfiguration : IEntityTypeConfiguration<Campus>
    {
        public void Configure(EntityTypeBuilder<Campus> builder)
        {
            // Table name
            builder.ToTable("Campuses");

            // Primary Key
            builder.HasKey(c => c.Id);

            // Property configurations
            builder.Property(c => c.Id)
                   .IsRequired()
                   .ValueGeneratedOnAdd();

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(c => c.Latitude)
                   .HasMaxLength(50);

            builder.Property(c => c.Longitude)
                   .HasMaxLength(50);

            builder.Property(c => c.Address)
                   .HasMaxLength(300);

            builder.Property(c => c.City)
                   .HasMaxLength(100);

            builder.Property(c => c.Zip)
                   .HasMaxLength(20);

            builder.Property(c => c.State)
                   .HasMaxLength(100);

            builder.Property(c => c.Country)
                   .HasMaxLength(100);

            builder.Property(c => c.GmapUrl)
                   .HasMaxLength(500);
        }
    }
}
