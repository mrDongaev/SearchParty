namespace DataAccess.Entities
{
    public class TeamPlayer
    {
        public Guid TeamId { get; set; }

        public Guid PlayerId { get; set; }

        public int PositionId { get; set; }

        public Position Position { get; set; }
    }
}
