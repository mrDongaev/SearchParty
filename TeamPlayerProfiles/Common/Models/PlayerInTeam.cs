using Common.Models.Enums;

namespace Common.Models
{
    public class PlayerInTeam
    {
        public PositionName Position { get; set; }

        public Guid PlayerId { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PlayerInTeam team &&
                Position == team.Position &&
                PlayerId.Equals(team.PlayerId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position, PlayerId);
        }
    }
}
