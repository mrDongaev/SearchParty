
namespace DataAccess.Entities
{
    public class TeamPlayer : IEquatable<TeamPlayer?>
    {
        public Guid TeamId { get; set; }

        public Team Team { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public Guid PlayerId { get; set; }

        public Player Player { get; set; }

        public int PositionId { get; set; }

        public Position Position { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as TeamPlayer);
        }

        public bool Equals(TeamPlayer? other)
        {
            return other is not null &&
                   PlayerId.Equals(other.PlayerId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PlayerId);
        }

        public static bool operator ==(TeamPlayer? left, TeamPlayer? right)
        {
            return EqualityComparer<TeamPlayer>.Default.Equals(left, right);
        }

        public static bool operator !=(TeamPlayer? left, TeamPlayer? right)
        {
            return !(left == right);
        }
    }
}
