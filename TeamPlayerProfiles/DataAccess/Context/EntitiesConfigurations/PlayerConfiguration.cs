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

            builder.HasAlternateKey(e => e.Id);

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.HasAlternateKey(e => e.Id);

            builder.HasIndex(e => e.Id)
                .IsUnique();

            builder.HasOne(e => e.Position)
                .WithMany()
                .HasForeignKey(e => e.PositionId)
                .IsRequired();

            builder.HasMany(e => e.Heroes)
                .WithMany(e => e.Players)
                .UsingEntity<PlayerHero>(
                e => e.HasOne<Hero>(e => e.Hero).WithMany(e => e.PlayerHeroes).HasForeignKey(e => e.HeroId),
                e => e.HasOne<Player>(e => e.Player).WithMany(e => e.PlayerHeroes).HasPrincipalKey(e => e.Id));

            base.Configure(builder);
        }
    }
}
