using Library.Models.Enums;
using Service.Contracts.Player;

namespace Service.Contracts.Team
{
    public static class TeamPlayerDto
    {
        public sealed class Write : IEquatable<Write?>
        {
            public PositionName Position { get; set; }

            public Guid PlayerId { get; set; }

            public override bool Equals(object? obj)
            {
                return Equals(obj as Write);
            }

            public bool Equals(Write? other)
            {
                return other is not null &&
                       PlayerId.Equals(other.PlayerId);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(PlayerId);
            }

            public static bool operator ==(Write? left, Write? right)
            {
                return EqualityComparer<Write>.Default.Equals(left, right);
            }

            public static bool operator !=(Write? left, Write? right)
            {
                return !(left == right);
            }
        }

        public sealed class Read
        {
            public PositionName Position { get; set; }

            public PlayerDto Player { get; set; }
        }
    }
}
