namespace Library.Models.API.UserMessaging
{
    public class GetPlayerInvitation
    {
        public sealed class Response : GetMessage.Response
        {
            public Guid InvitingTeamId { get; set; }

            public Guid AcceptingPlayerId { get; set; }
        }
    }
}
