using DataAccess.Context.EntitiesConfigurations;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    public class TeamPlayerProfilesContext : DbContext
    {
        public DbSet<Team> Teams { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Hero> Heroes { get; set; }

        public DbSet<Position> Positions { get; set; }

        public TeamPlayerProfilesContext(DbContextOptions<TeamPlayerProfilesContext> options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PositionConfiguration());
            modelBuilder.ApplyConfiguration(new HeroConfiguration());
            modelBuilder.ApplyConfiguration(new PlayerConfiguration());
            modelBuilder.ApplyConfiguration(new TeamConfiguration());
            modelBuilder.ApplyConfiguration(new TeamPlayerConfiguration());
        }
    }
}
