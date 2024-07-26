using Common.Models.Enums;

namespace Common.Models
{
    public class PlayerInTeam
    {
        public PositionName Position { get; set; }

        public Guid PlayerId { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PlayerInTeam pit && PlayerId.Equals(pit.PlayerId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PlayerId);
        }
    }
}
