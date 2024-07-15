using Service.Contracts.Player;

namespace Service.Contracts.Team
{
    public class TeamDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Displayed { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ICollection<PlayerInTeamDto> Players { get; set; }
    }
}
