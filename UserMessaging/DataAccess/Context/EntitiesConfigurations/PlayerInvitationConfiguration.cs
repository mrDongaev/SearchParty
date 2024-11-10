using DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Context.EntitiesConfigurations
{
    public class PlayerInvitationConfiguration : MessageConfiguration<PlayerInvitation>
    {
        public override void Configure(EntityTypeBuilder<PlayerInvitation> builder)
        {
            builder.Property(pi => pi.AcceptingPlayerId)
                .IsRequired();

            builder.Property(pi => pi.InvitingTeamId)
                .IsRequired();

            base.Configure(builder);
        }
    }
}
