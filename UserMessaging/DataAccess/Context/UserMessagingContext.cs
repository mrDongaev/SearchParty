using DataAccess.Context.EntitiesConfigurations;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    public class UserMessagingContext : DbContext
    {
        public DbSet<PlayerInvitationEntity> PlayerInvitations;

        public DbSet<TeamApplicationEntity> TeamApplications;

        public UserMessagingContext(DbContextOptions<UserMessagingContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PlayerInvitationConfiguration());
            modelBuilder.ApplyConfiguration(new TeamApplicationConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
