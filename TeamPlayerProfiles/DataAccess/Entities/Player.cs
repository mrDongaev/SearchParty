using DataAccess.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class Player : IProfile
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool? Displayed { get; set; }

        public DateTime UpdatedAt { get; set; }

        [NotMapped]
        public uint Mmr { get; set; }

        public int? PositionId { get; set; }

        public Position? Position { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public ICollection<Hero> Heroes { get; set; } = [];

        public ICollection<PlayerHero> PlayerHeroes { get; set; } = [];

        public ICollection<Team> Teams { get; set; } = [];

        public ICollection<TeamPlayer> TeamPlayers { get; set; } = [];
    }
}
