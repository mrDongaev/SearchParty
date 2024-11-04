using Library.Models.API.TeamPlayerProfiles.Hero;
using Library.Models.Enums;

namespace Library.Models.API.TeamPlayerProfiles.Player
{
    public static class GetPlayer
    {
        public sealed class Response
        {
            public Guid Id { get; set; }

            public Guid UserId { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public bool Displayed { get; set; }

            public PositionName Position { get; set; }

            public uint Mmr { get; set; }

            public DateTime UpdatedAt { get; set; }

            public ICollection<GetHero.Response> Heroes { get; set; }
        }
    }
}
