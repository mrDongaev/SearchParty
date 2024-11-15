using DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Context.EntitiesConfigurations
{
    public class TeamApplicationConfiguration : MessageConfiguration<TeamApplicationEntity>
    {
        public override void Configure(EntityTypeBuilder<TeamApplicationEntity> builder)
        {
            builder.Property(ta => ta.ApplyingPlayerId)
                .IsRequired();

            builder.Property(ta => ta.AcceptingTeamId)
                .IsRequired();

            base.Configure(builder);
        }
    }
}
