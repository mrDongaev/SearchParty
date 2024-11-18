namespace Service.Dtos.Message
{
    public class PlayerInvitationDto : MessageDto
    {
        public Guid InvitingTeamId { get; set; }

        public Guid AcceptingPlayerId { get; set; }
    }
}
