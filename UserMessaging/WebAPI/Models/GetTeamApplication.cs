using Library.Models.Enums;

namespace WebAPI.Models
{
    public static class GetTeamApplication
    {
        public sealed class Response
        {
            public Guid Id { get; set; }

            public Guid ApplyingPlayerId { get; set; }

            public Guid SendingUserId { get; set; }

            public Guid AcceptingTeamId { get; set; }

            public Guid AcceptingUserId { get; set; }

            public PositionName PositionName { get; set; }

            public MessageStatus Status { get; set; }

            public DateTime IssuedAt { get; set; }

            public DateTime ExpiresAt { get; set; }

            public DateTime UpdatedAt { get; set; }
        }
    }
}
