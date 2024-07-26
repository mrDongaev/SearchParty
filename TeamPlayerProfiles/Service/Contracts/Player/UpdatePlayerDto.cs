using Common.Models.Enums;

namespace Service.Contracts.Player
{
    public class UpdatePlayerDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public PositionName Position { get; set; }

        public ISet<int> HeroIds { get; set; }
    }
}
