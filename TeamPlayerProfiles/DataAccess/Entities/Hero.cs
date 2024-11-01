using Library.Entities.Interfaces;
using Library.Models.Enums;

namespace DataAccess.Entities
{
    public class Hero : IEntity<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public MainStat MainStat { get; set; }

        public ICollection<Player> Players { get; protected set; } = [];

        public ICollection<PlayerHero> PlayerHeroes { get; protected set; } = [];
    }
}
