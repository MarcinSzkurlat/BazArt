using Domain.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.DbConfiguration.User
{
    internal class UserCartProductConfiguration : IEntityTypeConfiguration<UserCartProduct>
    {
        public void Configure(EntityTypeBuilder<UserCartProduct> builder)
        {
            builder.HasOne(up => up.User)
                .WithMany(u => u.UserCartProducts)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(u => u.Product)
                .WithMany()
                .HasForeignKey(u => u.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
