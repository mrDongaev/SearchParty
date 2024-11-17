namespace Service.Dtos.Message
{
    public class TeamApplicationDto : PlayerInvitationDto
    {
        public Guid ApplyingPlayerId { get; set; }

        public Guid AcceptingTeamId { get; set; }
    }
}
