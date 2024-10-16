namespace Service.Contracts.Team
{
    public class CreateTeamDto
    {
        public Guid UserId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public ICollection<TeamPlayerService.Write> PlayersInTeam { get; set; } = [];
    }
}
