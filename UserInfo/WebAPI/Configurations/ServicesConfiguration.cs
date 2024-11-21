using Library.Utils;
using Service.Services.Implementations;
using Service.Services.Implementations.UserProfileServices;
using Service.Services.Interfaces;
using Service.Services.Interfaces.UserProfilesInterfaces;

namespace WebAPI.Configurations
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddScoped<IUserService, UserService>()
                .AddHttpClient<IUserProfileService, UserProfilesService>(cfg =>
                {
                    cfg.BaseAddress = new Uri(EnvironmentUtils.GetEnvVariable("TEAM_PLAYER_PROFILES_URL"));
                    cfg.DefaultRequestHeaders.Add("Accept", "text/plain");
                });
            return services;
        }
    }
}
