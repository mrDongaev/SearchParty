using Service.Contracts.Hero;

namespace Service.Contracts.Player
{
    public class UpdatePlayerDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Position { get; set; }

        public ICollection<HeroDto> Heroes { get; set; }
    }
}
