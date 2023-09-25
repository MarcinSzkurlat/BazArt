using Domain.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.DbConfiguration.User
{
    internal class FavoriteProductConfiguration : IEntityTypeConfiguration<FavoriteProduct>
    {
        public void Configure(EntityTypeBuilder<FavoriteProduct> builder)
        {
            builder.HasOne(lp => lp.User)
                .WithMany(u => u.FavoriteProducts)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(lp => lp.FavoriteProductObject)
                .WithMany()
                .HasForeignKey(u => u.FavoriteProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
