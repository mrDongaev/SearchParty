using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    public class UserProfilesContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserProfilesContext(DbContextOptions<UserProfilesContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<User>()
                .Property(e => e.Id);

            modelBuilder.Entity<User>()
                .Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
