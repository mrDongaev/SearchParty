using Library.Services.Implementations.AuthenticationServices;
using Library.Services.Implementations.UserContextServices;
using Library.Services.Interfaces.AuthenticationInterfaces;
using Library.Services.Interfaces.UserContextInterfaces;
using Library.Utils;
using Service.Services.Implementations;
using Service.Services.Implementations.HeroServices;
using Service.Services.Implementations.PlayerServices;
using Service.Services.Implementations.PositionServices;
using Service.Services.Implementations.TeamServices;
using Service.Services.Interfaces;
using Service.Services.Interfaces.HeroInterfaces;
using Service.Services.Interfaces.PlayerInterfaces;
using Service.Services.Interfaces.PositionInterfaces;
using Service.Services.Interfaces.TeamInterfaces;

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
                .AddScoped<IUserHttpContext, UserHttpContext>()
                .AddHttpClient<IAuthenticationService, AuthenticationService>(cfg =>
                {
                    cfg.BaseAddress = new Uri(EnvironmentUtils.GetEnvVariable("AUTHENTICATION_SERVICE_URL"));
                    cfg.DefaultRequestHeaders.Add("Accept", "text/plain");
                });
            return services;
        }
    }
}
