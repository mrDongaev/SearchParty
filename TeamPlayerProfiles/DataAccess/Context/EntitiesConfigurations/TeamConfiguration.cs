using DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context.Configurations
{
    public class TeamConfiguration : ProfileConfiguration<Team>
    {
        public override void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.Property(e => e.PlayerCount)
                .IsRequired()
                .HasDefaultValue(0);

            builder.HasMany(e => e.Players)
                .WithMany(e => e.Teams)
                .UsingEntity<TeamPlayer>();

            base.Configure(builder);
        }
    }
}
