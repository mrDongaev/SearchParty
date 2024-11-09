using DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Context.EntitiesConfigurations
{
    public class TeamApplicationConfiguration : MessageConfiguration<TeamApplication>
    {
        public override void Configure(EntityTypeBuilder<TeamApplication> builder)
        {
            builder.Property(ta => ta.ApplyingPlayerId)
                .IsRequired();

            builder.Property(ta => ta.AcceptingTeamId)
                .IsRequired();

            base.Configure(builder);
        }
    }
}
