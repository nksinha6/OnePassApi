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
            builder.ToTable("desk");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                .IsRequired();

            builder.Property(d => d.UnitId)
                .IsRequired();

            builder.Property(d => d.AdminPhone);

            builder.Property(d => d.AccessMode);

            builder.Property(d => d.AccessCategory);

            builder.HasCheckConstraint("CK_desk_admin_phone",
                @"admin_phone ~ '^(\+)?[0-9\s\-().]{7,25}$'");
        }
    }
}
