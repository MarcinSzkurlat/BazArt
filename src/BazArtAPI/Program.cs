using Application.Extensions;
using BazArtAPI.Extensions;
using BazArtAPI.Middleware;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApi(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    try
    {
        var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<Seeder>();
        await seeder.SeedAsync();
    }
    catch (Exception exception)
    {
        Log.Fatal($"Error while creating database | {exception}");
    }
}

app.UseHttpsRedirection();
app.UseCors("FrontendClient");

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
