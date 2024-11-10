using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    public class UserMessagingContext : DbContext
    {
        public DbSet<PlayerInvitation> PlayerInvitations;

        public DbSet<TeamApplication> TeamApplications;

        public UserMessagingContext(DbContextOptions<UserMessagingContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
