using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Context.EntitiesConfigurations
{
    public class PlayerConfiguration : ProfileConfiguration<Player>
    {
        public override void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.HasKey(e => new { e.Id, e.UserId });

            builder.HasIndex(e => e.Id)
                .IsUnique();

            builder.HasOne(e => e.Position)
                .WithMany()
                .HasForeignKey(e => e.PositionId)
                .IsRequired();

            builder.HasMany(e => e.Heroes)
                .WithMany()
                .UsingEntity(e => e.ToTable("PlayerHero"));

            base.Configure(builder);
        }
    }
}
