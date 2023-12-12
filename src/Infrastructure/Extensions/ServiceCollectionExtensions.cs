using Application.Interfaces;
using Application.Interfaces.Services;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repository;
using Infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<BazArtDbContext>(options =>
                options.UseNpgsql(configuration["BazArtDb_PostgreSQL"]));

            service.AddScoped<Seeder>();
            service.AddScoped<IEventRepository, EventRepository>();
            service.AddScoped<IProductRepository, ProductRepository>();
            service.AddScoped<ICategoryRepository, CategoryRepository>();
            service.AddScoped<IUserRepository, UserRepository>();
            service.AddScoped<IFavoriteProductsRepository, FavoriteProductsRepository>();
            service.AddScoped<IFavoriteUsersRepository, FavoriteUsersRepository>();
            service.AddScoped<ICartRepository, CartRepository>();
            service.AddScoped<IPhotoService, CloudinaryPhotoService>();
        }
    }
}
