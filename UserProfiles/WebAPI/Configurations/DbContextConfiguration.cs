using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Configurations
{
    public static class DbContextConfiguration
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            //if (CommonUtils.TryGetEnvVariable("CONTAINER").Equals("true"))
            //{
            //    string hostname = CommonUtils.GetEnvVariable("DATABASE_HOSTNAME");
            //    string portnum = CommonUtils.GetEnvVariable("DATABASE_PORT");
            //    string dbname = CommonUtils.GetEnvVariable("DATABASE_NAME");
            //    string username = CommonUtils.GetEnvVariable("DATABASE_USER");
            //    string password = CommonUtils.GetEnvVariable("DATABASE_PASSWORD");
            //    connectionString = $"Host={hostname};Port={portnum};Database={dbname};Username={username};Password={password}";
            //}
            services.AddDbContext<UserProfilesContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
            return services;
        }
    }
}
