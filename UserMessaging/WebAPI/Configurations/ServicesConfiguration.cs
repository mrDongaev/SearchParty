using Service.Services.Implementations.MessageProcessing;

namespace WebAPI.Configurations
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddTransient<SubmittedPlayerInvitationProcessor>()
                .AddTransient<SubmittedTeamApplicationProcessor>();
            return services;
        }
    }
}
