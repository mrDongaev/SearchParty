using DataAccess.Repositories.Implementations;
using DataAccess.Repositories.Interfaces;

namespace WebAPI.Configurations
{
    public static class RepositoriesConfiguration
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services
                .AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}
