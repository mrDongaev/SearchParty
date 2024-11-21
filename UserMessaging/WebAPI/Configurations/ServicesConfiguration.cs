using Library.Services.Implementations.UserContextServices;
using Library.Services.Interfaces.UserContextInterfaces;
using Library.Utils;
using Service.Services.Implementations.MessageInteraction;
using Service.Services.Implementations.MessageManagement;
using Service.Services.Implementations.MessageProcessing;
using Service.Services.Implementations.TeamServices;
using Service.Services.Interfaces.MessageInteraction;
using Service.Services.Interfaces.TeamInterfaces;

namespace WebAPI.Configurations
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddSingleton<TeamApplicationManager>()
                .AddSingleton<PlayerInvitationManager>()
                .AddScoped<IPlayerInvitationInteractionService, PlayerInvitationInteractionService>()
                .AddScoped<ITeamApplicationInteractionService, TeamApplicationInteractionService>()
                .AddTransient<SubmittedPlayerInvitationProcessor>()
                .AddTransient<SubmittedTeamApplicationProcessor>()
                .AddHttpClient<ITeamService, TeamService>(cfg =>
                {
                    cfg.BaseAddress = new Uri(EnvironmentUtils.GetEnvVariable("TEAM_PLAYER_PROFILES_URL"));
                    cfg.DefaultRequestHeaders.Add("Accept", "text/plain");
                });

            return services;
        }
    }
}
