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
using static Common.Models.ConditionalQuery;

namespace WebAPI.Configurations
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddScoped<IPositionService, PositionService>()
                .AddScoped<IHeroService, HeroService>()
                .AddScoped<IPlayerService, PlayerService>()
                .AddScoped<ITeamService, TeamService>()
                .AddScoped<IBoardService<PlayerDto, PlayerConditions>, PlayerBoardService>()
                .AddScoped<ITeamBoardService, TeamBoardService>();
            return services;
        }
    }
}
