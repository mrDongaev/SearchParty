using DataAccess.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class Team : IProfile
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool Displayed { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int PlayerCount { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        [NotMapped]
        public double AvgMmr { get; set; }

        public ICollection<Player> Players { get; set; } = [];

        public ICollection<TeamPlayer> TeamPlayers { get; set; } = [];
    }
}
