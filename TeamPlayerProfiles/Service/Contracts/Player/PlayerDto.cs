using Common.Models.Enums;
using Service.Contracts.Hero;

namespace Service.Contracts.Player
{
    public class PlayerDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Displayed { get; set; }

        public PositionName Position { get; set; }

        public ISet<HeroDto> Heroes { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
