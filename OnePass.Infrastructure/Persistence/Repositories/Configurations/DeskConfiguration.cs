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

            builder.Property(d => d.Id)
                   .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(d => d.Name)
                   .IsRequired();

            builder.Property(d => d.UnitId)
                   .IsRequired();

        }
    }
}
