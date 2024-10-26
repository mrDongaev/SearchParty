using Common.Enums;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
            int id = 1;
            Func<int> getId = () => id++;
            builder.HasData(new Position { Id = getId(), Name = PositionName.Carry });
            builder.HasData(new Position { Id = getId(), Name = PositionName.Midlane });
            builder.HasData(new Position { Id = getId(), Name = PositionName.Offlane });
            builder.HasData(new Position { Id = getId(), Name = PositionName.Roamer });
            builder.HasData(new Position { Id = getId(), Name = PositionName.Support });
        }
    }
}
