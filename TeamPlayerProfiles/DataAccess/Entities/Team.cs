using DataAccess.Entities.Interfaces;

namespace DataAccess.Entities
{
    public class Team : IProfile
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool? Displayed { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int PlayerCount { get; set; }

        public ICollection<Player> Players { get; protected set; } = [];

        public ICollection<TeamPlayer> TeamPlayers { get; protected set; } = [];
    }
}
