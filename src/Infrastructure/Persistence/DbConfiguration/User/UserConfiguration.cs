using Domain.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.DbConfiguration.User
{
    internal class UserConfiguration : IEntityTypeConfiguration<Domain.Models.User.User>
    {
        public void Configure(EntityTypeBuilder<Domain.Models.User.User> builder)
        {
            builder.Property(u => u.StageName)
                .HasMaxLength(50);
            builder.Property(u => u.Description)
                .HasMaxLength(1000);

            builder.HasOne(u => u.Category)
                .WithMany(c => c.Users)
                .HasForeignKey(k => k.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(u => u.Address)
                .WithOne(ua => ua.User)
                .HasForeignKey<UserAddress>(k => k.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.CreatedEvents)
                .WithOne(e => e.CreatedBy)
                .HasForeignKey(k => k.CreatedById)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.OwnedProducts)
                .WithOne(p => p.CreatedBy)
                .HasForeignKey(k => k.CreatedById)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
