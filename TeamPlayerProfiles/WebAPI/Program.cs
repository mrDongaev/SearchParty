using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WebAPI.Configurations;
using WebAPI.Middleware;

Log.Logger = new Serilog.LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();
Log.Information(Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? "none");
Log.Information(Environment.GetEnvironmentVariable("SEED_DATABASE") ?? "none");
Log.Information("Starting Team and Player profiles app...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services
        .AddDbContext<TeamPlayerProfilesContext>(options =>
        {
            options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
        })
        .AddRepositories()
        .AddServices()
        .AddAutoMapper()
        .AddEndpointsApiExplorer()
        .AddSwagger()
        .AddControllers();

    builder.Host
        .AddSerilog();

    var app = builder.Build();
    app.UseSwagger();
    app.UseSwaggerUI();
    if (Environment.GetEnvironmentVariable("SEED_DATABASE") != null && Environment.GetEnvironmentVariable("SEED_DATABASE").Equals("true"))
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<TeamPlayerProfilesContext>();
            await TestDataSeeder.SeedTestData(dbContext);
        }
    }
    app.UseSerilogRequestLogging();
    app.UseExceptionHandling();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    await app.RunAsync();
    Log.Information("Clean shutdown.");
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception has occured during bootstrapping.");
    return 1;
}
finally
{
    Log.Information("Shut down complete.");
    Log.CloseAndFlush();
}

