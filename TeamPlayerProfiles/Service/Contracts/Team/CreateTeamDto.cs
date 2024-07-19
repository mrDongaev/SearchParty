namespace Service.Contracts.Team
{
    public class CreateTeamDto
    {
        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<PlayerInTeamDto> Players { get; set; }
    }
}
