﻿using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Context.EntitiesConfigurations
{
    public class TeamPlayerConfiguration : IEntityTypeConfiguration<TeamPlayer>
    {
        public void Configure(EntityTypeBuilder<TeamPlayer> builder)
        {
            builder.HasIndex(tp => new { tp.TeamId, tp.PositionId })
                .IsUnique();

            builder.HasIndex(tp => new { tp.TeamId, tp.UserId })
                .IsUnique();

            builder.HasOne(tp => tp.User)
                .WithMany(u => u.TeamPlayers)
                .HasForeignKey(tp => tp.UserId);

            builder.HasOne(e => e.Position)
                .WithMany()
                .HasForeignKey(e => e.PositionId)
                .IsRequired();
        }
    }
}
