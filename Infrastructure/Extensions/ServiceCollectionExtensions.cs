using Infrastructure.Persistence;
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
                options.UseSqlServer(configuration.GetConnectionString("BazArtDb")));
            service.AddScoped<Seeder>();
        }
    }
}
