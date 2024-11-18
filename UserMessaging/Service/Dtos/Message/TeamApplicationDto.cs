namespace Service.Dtos.Message
{
    public class TeamApplicationDto : MessageDto
    {
        public Guid ApplyingPlayerId { get; set; }

        public Guid AcceptingTeamId { get; set; }
    }
}
