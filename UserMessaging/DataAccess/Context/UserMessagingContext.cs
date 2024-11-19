using DataAccess.Context.EntitiesConfigurations;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    public class UserMessagingContext : DbContext
    {
        public DbSet<PlayerInvitationEntity> PlayerInvitations { get; set; };

        public DbSet<TeamApplicationEntity> TeamApplications { get; set; }

        public UserMessagingContext(DbContextOptions<UserMessagingContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PlayerInvitationConfiguration());
            modelBuilder.ApplyConfiguration(new TeamApplicationConfiguration());
        }
    }
}
