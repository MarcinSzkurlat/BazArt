using Domain.Models;
using Domain.Models.Event;
using Domain.Models.User;
using Infrastructure.Persistence.DbConfiguration;
using Infrastructure.Persistence.DbConfiguration.Event;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.DbConfiguration.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class BazArtDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public BazArtDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<UserCartProduct> UsersCartProducts { get; set; }
        public DbSet<FavoriteUser> FavoriteUsers { get; set; }
        public DbSet<FavoriteProduct> FavoriteProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventDetail> EventDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserAddressConfiguration());
            modelBuilder.ApplyConfiguration(new UserCartProductConfiguration());
            modelBuilder.ApplyConfiguration(new FavoriteUserConfiguration());
            modelBuilder.ApplyConfiguration(new FavoriteProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new EventConfiguration());
            modelBuilder.ApplyConfiguration(new EventDetailConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        }
    }
}
