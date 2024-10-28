using DataAccess.Repositories.Implementations;
using DataAccess.Repositories.Interfaces;

namespace WebAPI.Configurations
{
    public static class RepositoriesConfiguration
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services
                .AddScoped<IPositionRepository, PositionRepository>()
                .AddScoped<IHeroRepository, HeroRepository>()
                .AddScoped<IPlayerRepository, PlayerRepository>()
                .AddScoped<ITeamRepository, TeamRepository>();
            return services;
        }
    }
}
