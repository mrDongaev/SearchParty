using Library.Utils;
using Service.Services.Implementations;
using Service.Services.Implementations.HeroServices;
using Service.Services.Implementations.PlayerServices;
using Service.Services.Implementations.PositionServices;
using Service.Services.Implementations.TeamServices;
using Service.Services.Implementations.UserProfileServices;
using Service.Services.Interfaces;
using Service.Services.Interfaces.HeroInterfaces;
using Service.Services.Interfaces.PlayerInterfaces;
using Service.Services.Interfaces.PositionInterfaces;
using Service.Services.Interfaces.TeamInterfaces;
using Service.Services.Interfaces.UserProfilesInterfaces;

namespace WebAPI.Configurations
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddScoped<IPositionService, PositionService>()
                .AddScoped<IHeroService, HeroService>()
                .AddScoped<IPlayerService, PlayerService>()
                .AddScoped<ITeamService, TeamService>()
                .AddScoped<IPlayerBoardService, PlayerBoardService>()
                .AddScoped<ITeamBoardService, TeamBoardService>()
                .AddScoped<IUserService, UserService>()
                .AddHttpClient<IUserProfileService, UserProfilesService>(cfg =>
                {
                    Uri baseAddress;
                    if (EnvironmentUtils.TryGetEnvVariable("CONTAINER").Equals("true"))
                    {
                        baseAddress = new Uri(EnvironmentUtils.GetEnvVariable("USER_PROFILES_URL"));
                    }
                    else
                    {
                        HttpClientSettings httpClientSettings = configuration.GetSection("HttpClientSettings").Get<HttpClientSettings>();
                        baseAddress = new Uri(httpClientSettings.UserProfilesUrl);
                    }
                    cfg.BaseAddress = baseAddress;
                    cfg.DefaultRequestHeaders.Add("Accept", "text/plain");
                });
            return services;
        }

        public class HttpClientSettings
        {
            public string UserProfilesUrl { get; set; }
        }
    }
}
