using DataAccess.Context;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using WebAPI.Configurations;
using WebAPI.Middleware;
using Serilog;
using System.Text.Json;
using System.Text;

Log.Logger = new Serilog.LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting Team and Player profiles app...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services
        .AddDbContext<TeamPlayerProfilesContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
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
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<TeamPlayerProfilesContext>();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
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

