using Library.Models.Enums;

namespace Service.Contracts.Player
{
    public class CreatePlayerDto
    {
        public Guid UserId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public PositionName Position { get; set; }

        public ISet<int> HeroIds { get; set; } = new HashSet<int>();
    }
}
