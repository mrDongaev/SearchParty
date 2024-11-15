namespace Service.Dtos
{
    public class PlayerInvitationDto : MessageDto
    {
        public Guid InvitingTeamId { get; set; }

        public Guid AcceptingPlayerId { get; set; }
    }
}
