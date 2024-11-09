using DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
