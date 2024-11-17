namespace Service.Dtos.Message
{
    public class PlayerInvitationDto : PlayerInvitationDto
    {
        public Guid InvitingTeamId { get; set; }

        public Guid AcceptingPlayerId { get; set; }
    }
}
