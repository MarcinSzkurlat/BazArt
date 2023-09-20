using Domain.Models.Event;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.DbConfiguration.Event
{
    internal class EventDetailConfiguration : IEntityTypeConfiguration<EventDetail>
    {
        public void Configure(EntityTypeBuilder<EventDetail> builder)
        {
            builder.Property(a => a.Country)
                .HasMaxLength(100);
            builder.Property(a => a.City)
                .HasMaxLength(100);
            builder.Property(a => a.Street)
                .HasMaxLength(100);
            builder.Property(a => a.HouseNumber)
                .HasMaxLength(10);
            builder.Property(a => a.PostalCode)
                .HasMaxLength(10);
            builder.Property(ed => ed.StartingDate)
                .IsRequired();
            builder.Property(ed => ed.EndingDate)
                .IsRequired();
        }
    }
}
