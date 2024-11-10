using DataAccess.Repositories.Implementations;
using DataAccess.Repositories.Interfaces;

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
