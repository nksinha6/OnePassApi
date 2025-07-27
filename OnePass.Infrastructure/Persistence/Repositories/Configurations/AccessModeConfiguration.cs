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
    public class AccessModeConfiguration : IEntityTypeConfiguration<AccessMode>
    {
        public void Configure(EntityTypeBuilder<AccessMode> builder)
        {
            // ✅ Table
            builder.ToTable("access_modes");

            // ✅ Primary Key
            builder.HasKey(am => am.Id);

            // ✅ Fields
            builder.Property(am => am.Id)
                   .IsRequired();

            builder.Property(am => am.Name)
                   .IsRequired();

            // ✅ Unique Constraint
            builder.HasIndex(am => am.Name)
                   .IsUnique();
        }
    }
}
