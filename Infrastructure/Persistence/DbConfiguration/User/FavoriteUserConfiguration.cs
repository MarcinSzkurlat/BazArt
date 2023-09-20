using Domain.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.DbConfiguration.User
{
    internal class FavoriteUserConfiguration : IEntityTypeConfiguration<FavoriteUser>
    {
        public void Configure(EntityTypeBuilder<FavoriteUser> builder)
        {
            builder.HasOne(lu => lu.User)
                .WithMany(u => u.FavoriteUsers)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(lu => lu.FavoriteUserObject)
                .WithMany()
                .HasForeignKey(u => u.FavoriteUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
