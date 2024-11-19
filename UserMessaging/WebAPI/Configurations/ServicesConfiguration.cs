using Library.Services.Implementations.UserContextServices;
using Library.Services.Interfaces.UserContextInterfaces;
using Service.Services.Implementations.MessageInteraction;
using Service.Services.Implementations.MessageManagement;
using Service.Services.Implementations.MessageProcessing;
using Service.Services.Interfaces.MessageInteraction;

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
                .AddTransient<SubmittedTeamApplicationProcessor>();

            return services;
        }
    }
}
