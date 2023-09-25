using Domain.Models.Event;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.DbConfiguration.Event
{
    internal class EventConfiguration : IEntityTypeConfiguration<Domain.Models.Event.Event>
    {
        public void Configure(EntityTypeBuilder<Domain.Models.Event.Event> builder)
        {
            builder.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(e => e.Description)
                .HasMaxLength(10000);
            builder.Property(e => e.ImageUrl)
                .HasMaxLength(500)
                .IsRequired();

            builder.HasOne(e => e.Category)
                .WithMany(c => c.Events)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.CreatedBy)
                .WithMany(u => u.CreatedEvents)
                .HasForeignKey(k => k.CreatedById);

            builder.HasOne(e => e.EventDetail)
                .WithOne(ed => ed.Event)
                .HasForeignKey<EventDetail>(k => k.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
