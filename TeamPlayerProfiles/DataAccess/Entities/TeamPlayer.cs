namespace DataAccess.Entities
{
    public class TeamPlayer
    {
        public Guid TeamId { get; set; }

        public Team Team { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public Guid PlayerId { get; set; }

        public Player Player { get; set; }

        public int PositionId { get; set; }

        public Position Position { get; set; }
    }
}
