using DataAccess.Context.EntitiesConfigurations;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Context.EntitiesConfigurations
{
    public class TeamPlayerConfiguration: IEntityTypeConfiguration<TeamPlayer>
    {
        public void Configure(EntityTypeBuilder<TeamPlayer> builder)
        {
            builder.HasOne(e => e.Position)
                .WithMany()
                .HasForeignKey(e => e.PositionId)
                .IsRequired();
        }
    }
}
