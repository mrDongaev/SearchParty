using DataAccess.Context;
using DataAccess.Repositories.Implementations;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Contracts.Player;
using Service.Contracts.Team;
using Service.Services.Implementations.HeroServices;
using Service.Services.Implementations.PlayerServices;
using Service.Services.Implementations.PositionServices;
using Service.Services.Implementations.TeamServices;
using Service.Services.Interfaces.Common;
using Service.Services.Interfaces.HeroInterfaces;
using Service.Services.Interfaces.PlayerInterfaces;
using Service.Services.Interfaces.PositionInterfaces;
using Service.Services.Interfaces.TeamInterfaces;
using System.Text.Json.Serialization;
using WebAPI.Mapping;
using RepoMapping = Service.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TeamPlayerProfilesContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IPositionRepository, PositionRepository>()
    .AddScoped<IHeroRepository, HeroRepository>()
    .AddScoped<IPlayerRepository, PlayerRepository>()
    .AddScoped<ITeamRepository, TeamRepository>();

builder.Services.AddAutoMapper(typeof(RepoMapping.HeroMappingProfile), typeof(RepoMapping.PositionMappingProfile),
    typeof(RepoMapping.PlayerMappingProfile), typeof(RepoMapping.TeamMappingProfile),
    typeof(HeroMappingProfile), typeof(PositionMappingProfile),
    typeof(PlayerMappingProfile), typeof(TeamMappingProfile))
    .AddScoped<IPositionService, PositionService>()
    .AddScoped<IHeroService, HeroService>()
    .AddScoped<IPlayerService, PlayerService>()
    .AddScoped<ITeamService, TeamService>()
    .AddScoped<IBoardService<PlayerDto>, PlayerBoardService>()
    .AddScoped<IBoardService<TeamDto>, TeamBoardService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type =>
    {
        var name = type.Name;
        var declaringName = type.DeclaringType?.Name ?? string.Empty;
        if (declaringName != string.Empty) declaringName += ".";
        return declaringName + name;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
