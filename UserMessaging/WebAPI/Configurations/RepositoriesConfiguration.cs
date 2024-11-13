using DataAccess.Repositories.Implementations;
using Service.Repositories.Interfaces;

namespace WebAPI.Configurations
{
    public static class RepositoriesConfiguration
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services
                .AddScoped<ITeamApplicationRepository, TeamApplicationRepository>()
                .AddScoped<IPlayerInvitationRepository, PlayerInvitationRepository>();
            return services;
        }
    }
}
