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
    public class InviteGuestConfiguration : IEntityTypeConfiguration<InviteGuest>
    {
        public void Configure(EntityTypeBuilder<InviteGuest> builder)
        {
            // ✅ Table name
            builder.ToTable("invite_guests");

            // ✅ Primary Key
            builder.HasKey(x => x.Id);

            // ✅ Properties
            builder.Property(x => x.Id)
                   .IsRequired();

            builder.Property(x => x.InviteId)
                   .IsRequired();

            builder.Property(x => x.GuestPhone)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(x => x.RsvpStatus)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(x => x.CheckinTime);

            builder.Property(x => x.CheckoutTime);

            builder.Property(x => x.CreatedAt)
                   .IsRequired();

            // ✅ Foreign Key (without navigation property)
            builder.HasIndex(x => x.InviteId);
        }
    }
}
