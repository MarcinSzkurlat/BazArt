using BazArtAPI.Middleware;

namespace BazArtAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApi(this IServiceCollection services)
        {
            services.AddScoped<ErrorHandlingMiddleware>();
        }
    }
}
