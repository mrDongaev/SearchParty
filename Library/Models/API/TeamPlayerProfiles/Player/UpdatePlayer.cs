using Library.Models.Enums;

namespace Library.Models.API.TeamPlayerProfiles.Player
{
    public static class UpdatePlayer
    {
        public sealed class Request
        {
            public string? Name { get; set; }

            public string? Description { get; set; }

            public PositionName? Position { get; set; }
        }
    }
}
