using DataAccess.Entities;
using Library.Models.Enums;
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
            builder.HasData(new Hero { Id = getId(), Name = "Axe", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Anti-Mage", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Bane", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Bloodseeker", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Earthshaker", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Drow Ranger", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Storm Spirit", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Sven", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Tiny", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Vengeful Spirit", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Windranger", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Zeus", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Kunkka", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Lina", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Lion", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Shadow Shaman", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Slardar", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Tidehunter", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Witch Doctor", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Lich", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Riki", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Enigma", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Tinker", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Sniper", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Necrophos", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Warlock", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Beastmaster", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Queen of Pain", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Venomancer", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Faceless Void", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Wraith King", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Death Prophet", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Phantom Assassin", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Templar Assassin", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Viper", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Luna", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Dragon Knight", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Dazzle", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Clockwerk", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Leshrac", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Nature's Prophet", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Lifestealer", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Dark Seer", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Clinkz", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Omniknight", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Enchantress", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Huskar", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Night Stalker", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Broodmother", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Bounty Hunter", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Weaver", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Jakiro", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Batrider", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Chen", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Spectre", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Doom", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Ancient Apparition", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Ursa", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Spirit Breaker", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Gyrocopter", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Alchemist", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Silencer", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Outworld Destroyer", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Lycan", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Brewmaster", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Shadow Demon", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Lone Druid", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Chaos Knight", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Treant Protector", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Ogre Magi", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Undying", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Rubick", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Disruptor", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Nyx Assassin", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Naga Siren", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Keeper of the Light", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Io", MainStat = MainStat.Universal });
            builder.HasData(new Hero { Id = getId(), Name = "Visage", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Slark", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Medusa", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Troll Warlord", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Centaur Warrunner", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Magnus", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Timbersaw", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Bristleback", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Tusk", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Skywrath Mage", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Abaddon", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Elder Titan", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Legion Commander", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Ember Spirit", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Earth Spirit", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Terrorblade", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Phoenix", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Oracle", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Winter Wyvern", MainStat = MainStat.Universal });
            builder.HasData(new Hero { Id = getId(), Name = "Arc Warden", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Underlord", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Monkey King", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Dark Willow", MainStat = MainStat.Universal });
            builder.HasData(new Hero { Id = getId(), Name = "Pangolier", MainStat = MainStat.Universal });
            builder.HasData(new Hero { Id = getId(), Name = "Grimstroke", MainStat = MainStat.Intelligence });
            builder.HasData(new Hero { Id = getId(), Name = "Hoodwink", MainStat = MainStat.Agility });
            builder.HasData(new Hero { Id = getId(), Name = "Void Spirit", MainStat = MainStat.Universal });
            builder.HasData(new Hero { Id = getId(), Name = "Snapfire", MainStat = MainStat.Universal });
            builder.HasData(new Hero { Id = getId(), Name = "Mars", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Dawnbreaker", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Primal Beast", MainStat = MainStat.Strength });
            builder.HasData(new Hero { Id = getId(), Name = "Muerta", MainStat = MainStat.Intelligence });
        }
    }
}
