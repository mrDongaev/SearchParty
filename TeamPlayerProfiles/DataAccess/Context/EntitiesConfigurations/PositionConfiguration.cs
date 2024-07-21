using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Common.Models.Enums;

namespace DataAccess.Context.EntitiesConfigurations
{
    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.Property(e => e.Name)
                .IsRequired()
                .HasConversion(e => e.ToString(), e => Enum.Parse<PositionName>(e));

            SeedData(builder);
        }

        private void SeedData(EntityTypeBuilder<Position> builder)
        {
            builder.HasData(new Position { Id = 1, Name = PositionName.Carry });
            builder.HasData(new Position { Id = 2, Name = PositionName.Midlane });
            builder.HasData(new Position { Id = 3, Name = PositionName.Offlane });
            builder.HasData(new Position { Id = 4, Name = PositionName.Roamer });
            builder.HasData(new Position { Id = 5, Name = PositionName.Support });
        }
    }
}
