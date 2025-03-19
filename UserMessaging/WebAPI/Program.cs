using DataAccess.Context;
using Library.Configurations;
using Library.Middleware;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text.Json.Serialization;
using WebAPI.Configurations;
using WebAPI.Middleware;

Log.Logger = new Serilog.LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();
Log.Information("Starting User messaging app...");
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services
        .AddDbContext()
        .AddRepositories()
        .AddServices()
        .AddAutoMapper()
        .AddEndpointsApiExplorer()
        .AddSwagger("v1", new OpenApiInfo
        {
            Description = "SearchParty User Messaging API v1",
            Title = "User Messaging",
            Version = "1.0.0"
        })
        .AddAuthenticationConfiguration()
        .AddRabbitMQ()
        .AddJsonConfiguration()
        .AddControllers()
        .AddValidationResponseConfiguration();

    builder.Services.AddHealthChecks();

    builder.Host
        .AddSerilog();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<UserMessagingContext>();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }
    }

    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSerilogRequestLogging();
    app.UseExceptionHandling();
    app.UseHttpsRedirection();
    app.UseMiddleware<TokenRefreshMiddleware>();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseMiddleware<UserHttpContextMiddleware>();
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

