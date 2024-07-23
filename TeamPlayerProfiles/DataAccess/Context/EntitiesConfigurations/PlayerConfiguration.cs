using DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Context.EntitiesConfigurations
{
    public class PlayerConfiguration : ProfileConfiguration<Player>
    {
        public override void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.HasOne(e => e.Position)
                .WithMany()
                .HasForeignKey(e => e.PositionId)
                .IsRequired();

            builder.HasMany(e => e.Heroes)
                .WithMany();

            base.Configure(builder);
        }
    }
}
