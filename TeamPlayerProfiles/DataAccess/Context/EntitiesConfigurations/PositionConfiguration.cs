using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EnumPosition = Common.Models.Enums.Position;

namespace DataAccess.Context.EntitiesConfigurations
{
    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.Property(e => e.Name)
                .IsRequired()
                .HasConversion(e => e.ToString(), e => Enum.Parse<EnumPosition>(e));

            SeedData(builder);
        }

        private void SeedData(EntityTypeBuilder<Position> builder)
        {
            builder.HasData(new Position { Id = 1, Name = EnumPosition.Carry });
            builder.HasData(new Position { Id = 2, Name = EnumPosition.Midlane });
            builder.HasData(new Position { Id = 3, Name = EnumPosition.Offlane });
            builder.HasData(new Position { Id = 4, Name = EnumPosition.Roamer });
            builder.HasData(new Position { Id = 5, Name = EnumPosition.Support });
        }
    }
}
