using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Infrastructure.Persistence
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using OnePass.Domain;

    public class ZoneLevelConfiguration : IEntityTypeConfiguration<ZoneLevel>
    {
        public void Configure(EntityTypeBuilder<ZoneLevel> builder)
        {
            builder.ToTable("zone_levels");

            builder.HasKey(z => z.Id);

            builder.Property(z => z.Name)
                   .IsRequired()
                   .HasMaxLength(100);
        }
    }

}
