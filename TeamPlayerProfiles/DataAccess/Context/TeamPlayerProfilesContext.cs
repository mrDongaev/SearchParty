using Common.Utils;
using DataAccess.Context.EntitiesConfigurations;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DataAccess.Context
{
    public class TeamPlayerProfilesContext : DbContext
    {
        public DbSet<Team> Teams { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Hero> Heroes { get; set; }

        public DbSet<Position> Positions { get; set; }

        public TeamPlayerProfilesContext(DbContextOptions<TeamPlayerProfilesContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new PositionConfiguration());
            modelBuilder.ApplyConfiguration(new HeroConfiguration());
            modelBuilder.ApplyConfiguration(new PlayerConfiguration());
            modelBuilder.ApplyConfiguration(new TeamConfiguration());
            modelBuilder.ApplyConfiguration(new TeamPlayerConfiguration());
        }
    }

    public static class DbContextExtensions
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, string? defaultConnectionString)
        {
            var connectionString = defaultConnectionString;
            if (CommonUtils.TryGetEnvVariable("CONTAINER").Equals("true"))
            {
                string hostname = CommonUtils.GetEnvVariable("DATABASE_HOSTNAME");
                string portnum = CommonUtils.GetEnvVariable("DATABASE_PORT");
                string dbname = CommonUtils.GetEnvVariable("DATABASE_NAME");
                string username = CommonUtils.GetEnvVariable("DATABASE_USER");
                string password = CommonUtils.GetEnvVariable("DATABASE_PASSWORD");
                connectionString = $"Host={hostname};Port={portnum};Database={dbname};Username={username};Password={password}";
            }
            services.AddDbContext<TeamPlayerProfilesContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
            return services;
        }
    }
}
