using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.DbConfiguration
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(p => p.Description)
                .HasMaxLength(1000);
            builder.Property(p => p.Price)
                .HasPrecision(10, 2);
            builder.Property(p => p.IsForSell)
                .IsRequired();
            builder.Property(p => p.Quantity)
                .HasDefaultValue(1);
            builder.Property(p => p.ImageUrl)
                .HasMaxLength(500)
                .IsRequired();

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(k => k.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.CreatedBy)
                .WithMany(u => u.OwnedProducts)
                .HasForeignKey(k => k.CreatedById);
        }
    }
}
