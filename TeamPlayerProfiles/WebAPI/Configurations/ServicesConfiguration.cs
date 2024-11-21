using Library.Utils;
using Service.Services.Implementations;
using Service.Services.Implementations.HeroServices;
using Service.Services.Implementations.MessageServices;
using Service.Services.Implementations.PlayerServices;
using Service.Services.Implementations.PositionServices;
using Service.Services.Implementations.TeamServices;
using Service.Services.Implementations.UserServices;
using Service.Services.Interfaces;
using Service.Services.Interfaces.HeroInterfaces;
using Service.Services.Interfaces.MessageInterfaces;
using Service.Services.Interfaces.PlayerInterfaces;
using Service.Services.Interfaces.PositionInterfaces;
using Service.Services.Interfaces.TeamInterfaces;
using Service.Services.Interfaces.UserInterfaces;

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
                .AddScoped<IPlayerBoardService, PlayerBoardService>()
                .AddScoped<ITeamBoardService, TeamBoardService>()
                .AddScoped<IUserService, UserService>()
                .AddTransient<IUserIdentityService, UserIdentityService>();
            services
                .AddHttpClient<IPlayerInvitationService, PlayerInvitationService>(cfg =>
            {
                cfg.BaseAddress = new Uri(EnvironmentUtils.GetEnvVariable("USER_MESSAGING_URL"));
                cfg.DefaultRequestHeaders.Add("Accept", "text/plain");
            });
            services
                .AddHttpClient<ITeamApplicationService, TeamApplicationService>(cfg =>
            {
                cfg.BaseAddress = new Uri(EnvironmentUtils.GetEnvVariable("USER_MESSAGING_URL"));
                cfg.DefaultRequestHeaders.Add("Accept", "text/plain");
            });
            return services;
        }
    }
}
