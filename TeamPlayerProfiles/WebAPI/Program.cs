using DataAccess.Context;
using Library.Utils;
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
        .AddDbContext(builder.Configuration)
        .AddRepositories()
        .AddServices(builder.Configuration)
        .AddAutoMapper()
        .AddEndpointsApiExplorer()
        .AddSwagger();
        //.AddRabbitMQ(builder.Configuration);

    builder.Services
        .AddControllers();

    builder.Host
        .AddSerilog();

    var app = builder.Build();
    app.UseSwagger();
    app.UseSwaggerUI();
    if (EnvironmentUtils.TryGetEnvVariable("TEAM_PLAYER_PROFILES__SEED_DATABASE").Equals("true"))
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
    app.UseAuthorization();
    app.MapControllers();
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

