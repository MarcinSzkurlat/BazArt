using Application.Extensions;
using BazArtAPI.Extensions;
using BazArtAPI.Middleware;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

//Necessary secret environments for production version:
//AdminEmail
//AdminPassword
//BazArtDb_PostgreSQL
//CloudinaryApiKey
//CloudinaryApiSecret
//CloudinaryCloudName
//TokenKey (minimum 64 characters)

Log.Logger = new LoggerConfiguration()
    .WriteTo.Logger(lc =>
    {
        lc.Filter.ByIncludingOnly(le => le.Level == LogEventLevel.Information);
        lc.WriteTo.File("Logs/Info/info_.txt", rollingInterval: RollingInterval.Day);

    })
    .WriteTo.Logger(lc =>
    {
        lc.MinimumLevel.Warning();
        lc.WriteTo.File("Logs/Errors/error_.txt", rollingInterval: RollingInterval.Day);
    })
    .CreateLogger();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApi(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Docker")
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors("FrontendClient");
}

try
{
    var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<Seeder>();
    await seeder.SeedAsync(app.Environment);
}
catch (Exception exception)
{
    Log.Fatal($"Error while creating database | {exception}");
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();
app.MapFallbackToController("Index", "Fallback");

app.Run();
