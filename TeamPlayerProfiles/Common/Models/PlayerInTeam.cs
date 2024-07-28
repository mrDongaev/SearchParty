using Common.Models.Enums;

namespace Common.Models
{
    public class PlayerInTeam : IEquatable<PlayerInTeam?>
    {
        public PositionName Position { get; set; }

        public Guid PlayerId { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PlayerInTeam);
        }

        public bool Equals(PlayerInTeam? other)
        {
            return other is not null &&
                PlayerId.Equals(other.PlayerId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PlayerId);
        }
    }
}
