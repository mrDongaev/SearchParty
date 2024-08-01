using DataAccess.Entities.Interfaces;

namespace DataAccess.Entities
{
    public class Player : IProfile
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool? Displayed { get; set; }

        public int? PositionId { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Position? Position { get; set; }

        public ICollection<Hero> Heroes { get; protected set; } = [];

        public ICollection<Team> Teams { get; protected set; } = [];

        public ICollection<TeamPlayer> TeamPlayers { get; protected set; } = [];
    }
}
