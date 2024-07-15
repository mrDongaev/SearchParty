using DataAccess.Entities.Interfaces;

namespace DataAccess.Entities
{
    public class Player : IProfile
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Displayed { get; set; }

        public Position Position { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int PositionId { get; set; }

        public ICollection<Hero> Heroes { get; set; } = [];

        public ICollection<Team> Teams { get; set; } = [];

        public ICollection<TeamPlayer> TeamPlayers { get; set; } = [];
    }
}
