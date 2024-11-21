using DataAccess.Context;
using Library.Utils;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Configurations
{
    public static class DbContextConfiguration
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services)
        {
            string hostname = EnvironmentUtils.GetEnvVariable("DATABASE_HOSTNAME");
            string portnum = EnvironmentUtils.GetEnvVariable("DATABASE_PORT");
            string dbname = EnvironmentUtils.GetEnvVariable("DATABASE_NAME");
            string username = EnvironmentUtils.GetEnvVariable("DATABASE_USER");
            string password = EnvironmentUtils.GetEnvVariable("DATABASE_PASSWORD");
            var connectionString = $"Host={hostname};Port={portnum};Database={dbname};Username={username};Password={password}";

            services.AddDbContext<UserMessagingContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
            return services;
        }
    }
}
