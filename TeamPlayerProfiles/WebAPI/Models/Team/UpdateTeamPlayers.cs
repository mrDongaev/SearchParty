using Library.Models.Enums;
using System.ComponentModel.DataAnnotations;
using WebAPI.Models.Player;

namespace WebAPI.Models.Team
{
    public static class UpdateTeamPlayers
    {
        public sealed class Request : IEquatable<Request?>
        {
            [Required]
            public PositionName? Position { get; set; }

            [Required]
            public Guid? PlayerId { get; set; }

            public override bool Equals(object? obj)
            {
                return Equals(obj as Request);
            }

            public bool Equals(Request? other)
            {
                return other is not null && EqualityComparer<Guid?>.Default.Equals(PlayerId, other.PlayerId);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(PlayerId);
            }
        }

        public sealed class Response
        {
            public PositionName Position { get; set; }

            public GetPlayer.Response Player { get; set; }
        }
    }
}
