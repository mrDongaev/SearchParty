namespace Library.Models.API.UserMessaging
{
    public static class GetTeamApplication
    {
        public sealed class Response : GetMessage.Response
        {
            public Guid ApplyingPlayerId { get; set; }

            public Guid AcceptingTeamId { get; set; }
        }
    }
}
