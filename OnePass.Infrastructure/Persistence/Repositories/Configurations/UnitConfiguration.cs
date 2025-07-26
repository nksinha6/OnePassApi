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

    public class UnitConfiguration : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            builder.ToTable("unit");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                   .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(u => u.Name)
                   .IsRequired();

            builder.Property(u => u.CompanyId)
                   .IsRequired();
        }
    }
}
