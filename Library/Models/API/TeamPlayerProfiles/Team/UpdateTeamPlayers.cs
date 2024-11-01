using Library.Models.API.TeamPlayerProfiles.Player;
using Library.Models.Enums;

namespace Library.Models.API.TeamPlayerProfiles.Team
{
    public static class UpdateTeamPlayers
    {
        public sealed class Request
        {
            public PositionName? Position { get; set; }

            public Guid? PlayerId { get; set; }
        }

        public sealed class Response
        {
            public PositionName Position { get; set; }

            public GetPlayer.Response Player { get; set; }
        }
    }
}
