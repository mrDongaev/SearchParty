using DataAccess.Context;
using DataAccess.Repositories.Implementations;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Services.Implementations;
using Service.Services.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebAPI.Mapping;
using RepoMapping = Service.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TeamPlayerProfilesContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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
    .AddScoped<ITeamService, TeamService>();

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
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
