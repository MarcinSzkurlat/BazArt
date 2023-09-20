using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.DbConfiguration
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Domain.Models.Category>
    {
        public void Configure(EntityTypeBuilder<Domain.Models.Category> builder)
        {
            builder.Property(c => c.Name)
                .HasMaxLength(100);
            builder.Property(c => c.Description)
                .HasMaxLength(1000);
            builder.Property(c => c.ImageUrl)
                .HasMaxLength(500);

            builder.HasMany(c => c.Users)
                .WithOne(c => c.Category)
                .HasForeignKey(k => k.CategoryId);

            builder.HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(k => k.CategoryId);

            builder.HasMany(c => c.Events)
                .WithOne(e => e.Category)
                .HasForeignKey(k => k.CategoryId);
        }
    }
}
