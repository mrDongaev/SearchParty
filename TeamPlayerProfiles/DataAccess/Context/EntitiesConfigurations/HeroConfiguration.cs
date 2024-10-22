using Common.Models.Enums;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Context.EntitiesConfigurations
{
    public class HeroConfiguration : IEntityTypeConfiguration<Hero>
    {
        public void Configure(EntityTypeBuilder<Hero> builder)
        {
            builder.Property(e => e.Name)
                .IsRequired();

            builder.Property(e => e.MainStat)
                .IsRequired()
                .HasConversion(e => e.ToString(), e => Enum.Parse<MainStat>(e));

            SeedData(builder);
        }

        private static void SeedData(EntityTypeBuilder<Hero> builder)
        {
            int id = 1;
            Func<int> getId = () => id++;
            builder.HasData(new Hero { Id = getId(), Name = "Juggernaut", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Pudge", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Crystal Maiden", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Invoker", MainStat = MainStat.Universal });
            builder.HasData(new Hero { Id = getId(), Name = "Meepo", MainStat = MainStat.Agility });
        }
    }
}
