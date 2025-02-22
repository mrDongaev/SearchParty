using DataAccess.Context;
using Library.Configurations;
using Library.Middleware;
using Library.Models.HttpResponses;
using Library.Utils;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using WebAPI.Configurations;
using WebAPI.Middleware;

Log.Logger = new Serilog.LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();
Log.Information("Starting Team and Player profiles app...");
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services
        .AddDbContext()
        .AddRepositories()
        .AddServices()
        .AddAutoMapper()
        .AddEndpointsApiExplorer()
        .AddSwagger()
        .AddAuthenticationConfiguration()
        //.AddRabbitMQ()
        .AddControllers()
        .ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "One or more validation errors occurred.",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Please refer to the errors property for additional details.",
                    Instance = context.HttpContext.Request.Path
                };

                // Customize the response body
                var response = new HttpResponseBody
                {
                    IsSuccess = false,
                    Errors = problemDetails.Errors,
                };

                return new BadRequestObjectResult(response)
                {
                    ContentTypes = { "application/json" }
                };
            };
        });

    builder.Services.AddHealthChecks();

    builder.Host
        .AddSerilog();

    var app = builder.Build();
    app.UseSwagger();
    app.UseSwaggerUI();
    if (EnvironmentUtils.TryGetEnvVariable("TEAM_PLAYER_PROFILES__SEED_DATABASE", out var doSeed) && doSeed == "true")
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<TeamPlayerProfilesContext>();
            await TestDataSeeder.SeedTestData(dbContext);
            Log.Information("Team Player Profiles test data seeded");
        }
    }
    else if (app.Environment.IsDevelopment())
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<TeamPlayerProfilesContext>();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            await TestDataSeeder.SeedTestData(dbContext);
            Log.Information("Team Player Profiles test data seeded");
        }
    }
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

