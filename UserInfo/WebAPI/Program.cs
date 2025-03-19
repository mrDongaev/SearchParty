using DataAccess.Context;
using Library.Configurations;
using Library.Middleware;
using Library.Utils;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using System.Text.Json.Serialization;
using WebAPI.Configurations;
using WebAPI.Middleware;

Log.Logger = new Serilog.LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();
Log.Information("Starting User info app...");
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services
        .AddDbContext(builder.Configuration)
        .AddRepositories()
        .AddServices(builder.Configuration)
        .AddAutoMapper()
        .AddEndpointsApiExplorer()
        .AddSwagger()
        .AddAuthenticationConfiguration()
        .AddJsonConfiguration()
        .AddControllers()
        .AddValidationResponseConfiguration();

    builder.Services.AddHealthChecks();

    builder.Host
        .AddSerilog();

    var app = builder.Build();
    app.UseSwagger();
    app.UseSwaggerUI();
    if (EnvironmentUtils.TryGetEnvVariable("USER_INFO__SEED_DATABASE", out var doSeed) && doSeed == "true")
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<UserProfilesContext>();
            await TestDataSeeder.SeedTestData(dbContext);
            Log.Information("User profiles test data seeded");
        }
    }
    else if (app.Environment.IsDevelopment())
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<UserProfilesContext>();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            await TestDataSeeder.SeedTestData(dbContext);
            Log.Information("User profiles test data seeded");
        }
    }
    app.UseSerilogRequestLogging();
    app.UseExceptionHandling();
    app.UseHttpsRedirection();
    app.UseMiddleware<TokenRefreshMiddleware>();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseMiddleware<UserHttpContextMiddleware>();
    app.UseAuthorization();
    app.MapControllers();
    app.MapHealthChecks("/healthcheck", new HealthCheckOptions
    {
        ResultStatusCodes =
        {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Degraded] = StatusCodes.Status200OK,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
        }
    });
    await app.RunAsync();
    Log.Information("Clean shutdown.");
    return 0;
}
catch (Exception ex)
{
    if (ex is not HostAbortedException)
        Log.Fatal(ex, "An unhandled exception has occured during bootstrapping.");
    return 1;
}
finally
{
    Log.Information("Shut down complete.");
    Log.CloseAndFlush();
}
