using Domain.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.DbConfiguration.User
{
    internal class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
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
        }
    }
}
