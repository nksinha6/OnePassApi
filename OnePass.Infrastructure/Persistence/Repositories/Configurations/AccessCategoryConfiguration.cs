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
    public class AccessCategoryConfiguration : IEntityTypeConfiguration<AccessCategory>
    {
        public void Configure(EntityTypeBuilder<AccessCategory> builder)
        {
            // ✅ Table
            builder.ToTable("access_categories");

            // ✅ Primary Key
            builder.HasKey(ac => ac.Id);

            // ✅ Fields
            builder.Property(ac => ac.Id)
                   .IsRequired();

            builder.Property(ac => ac.Name)
                   .IsRequired();

            // ✅ Unique Constraint
            builder.HasIndex(ac => ac.Name)
                   .IsUnique();
        }
    }
}
