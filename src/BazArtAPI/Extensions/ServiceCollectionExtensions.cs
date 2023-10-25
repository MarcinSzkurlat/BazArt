using System.Text;
using BazArtAPI.Dtos.User.Validators;
using BazArtAPI.Middleware;
using BazArtAPI.Services;
using Domain.Models.User;
using FluentValidation;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace BazArtAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ErrorHandlingMiddleware>();

            services.AddCors(opt =>
            {
                opt.AddPolicy("FrontendClient", policy =>
                {
                    policy.AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithOrigins(configuration["AllowedOrigin"]!);
                });
            });

            AddIdentity(services, configuration);

            services.AddScoped<AccountService>();
            services.AddScoped<TokenService>();
            services.AddValidatorsFromAssemblyContaining<LoginDtoValidator>();
        }

        private static void AddIdentity(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers(opt =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddIdentity<User, IdentityRole<Guid>>(opt =>
                {
                    opt.User.RequireUniqueEmail = true;
                })
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<BazArtDbContext>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));

            services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }
    }
}
