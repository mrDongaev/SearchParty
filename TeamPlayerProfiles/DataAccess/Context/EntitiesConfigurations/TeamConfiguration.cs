using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Context.EntitiesConfigurations
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
                .UsingEntity<TeamPlayer>(
                    e => e.HasOne<Player>(e => e.Player).WithMany(e => e.TeamPlayers).HasPrincipalKey(e => e.Id),
                    e => e.HasOne<Team>(e => e.Team).WithMany(e =>e.TeamPlayers).HasForeignKey(e => e.TeamId));

            base.Configure(builder);
        }
    }
}
