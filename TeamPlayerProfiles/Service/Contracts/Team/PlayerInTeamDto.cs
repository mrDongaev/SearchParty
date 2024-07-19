namespace Service.Contracts.Team
{
    public class PlayerInTeamDto
    {
        public string Position { get; set; }

        public Guid PlayerId { get; set; }
    }
}
