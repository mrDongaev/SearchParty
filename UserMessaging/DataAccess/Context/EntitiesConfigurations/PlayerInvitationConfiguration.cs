using DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Context.EntitiesConfigurations
{
    public class PlayerInvitationConfiguration : MessageConfiguration<PlayerInvitationEntity>
    {
        public override void Configure(EntityTypeBuilder<PlayerInvitationEntity> builder)
        {
            builder.Property(pi => pi.AcceptingPlayerId)
                .IsRequired();

            builder.Property(pi => pi.InvitingTeamId)
                .IsRequired();

            base.Configure(builder);
        }
    }
}
