using Library.Models.Enums;

namespace Library.Models.API.TeamPlayerProfiles.Team
{
    public static class PushPlayer
    {
        public sealed class Request
        {
            public Guid? MessageId { get; set; }

            public Guid PlayerId { get; set; }

            public Guid TeamId { get; set; }

            public PositionName Position { get; set; }

            public MessageType? MessageType { get; set; }
        }
    }
}
