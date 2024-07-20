using EnumPosition = Common.Models.Enums.Position;

namespace Service.Contracts.Team
{
    public class PlayerInTeamDto
    {
        public EnumPosition Position { get; set; }

        public Guid PlayerId { get; set; }
    }
}
