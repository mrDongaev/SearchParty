using DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Enums = DataAccess.Entities.Enums;

namespace DataAccess.Context.Configurations
{
    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.Property(e => e.Name)
                .IsRequired()
                .HasConversion(e => e.ToString(), e => Enum.Parse<Enums.Position>(e));

            builder.HasMany(e => e.Players)
                .WithOne(e => e.Position)
                .HasForeignKey(e => e.PositionId)
                .IsRequired();

            builder.HasMany(e => e.TeamPlayers)
                .WithOne(e => e.Position)
                .HasForeignKey(e => e.PositionId)
                .IsRequired();

            SeedData(builder);
        }

        private void SeedData(EntityTypeBuilder<Position> builder)
        {
            builder.HasData(new Position { Id = 1, Name = Enums.Position.Carry });
            builder.HasData(new Position { Id = 2, Name = Enums.Position.Midlane });
            builder.HasData(new Position { Id = 3, Name = Enums.Position.Offlane });
            builder.HasData(new Position { Id = 4, Name = Enums.Position.Roamer });
            builder.HasData(new Position { Id = 5, Name = Enums.Position.Support });
        }
    }
}
