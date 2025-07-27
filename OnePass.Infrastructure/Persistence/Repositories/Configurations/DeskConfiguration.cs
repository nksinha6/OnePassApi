using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnePass.Domain;

namespace OnePass.Infrastructure.Persistence
{
    public class DeskConfiguration : IEntityTypeConfiguration<Desk>
    {
        public void Configure(EntityTypeBuilder<Desk> builder)
        {
            // ✅ Table name
            builder.ToTable("desks");

            // ✅ Primary key
            builder.HasKey(d => d.Id);

            // ✅ Fields
            builder.Property(d => d.Id)
                   .IsRequired();

            builder.Property(d => d.Name)
                   .IsRequired();

            builder.Property(d => d.UnitId)
                   .IsRequired();

            builder.Property(d => d.AdminPhone)
                   .IsRequired(false);

            builder.Property(d => d.AccessModeId)
                   .IsRequired();

            builder.Property(d => d.AccessCategoryId)
                   .IsRequired();

            // ✅ Foreign Keys (no navigation properties)
            builder.HasOne<Unit>()
                   .WithMany()
                   .HasForeignKey(d => d.UnitId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(d => d.AdminPhone)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<AccessMode>()
                   .WithMany()
                   .HasForeignKey(d => d.AccessModeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<AccessCategory>()
                   .WithMany()
                   .HasForeignKey(d => d.AccessCategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
