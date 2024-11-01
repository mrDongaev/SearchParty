using Library.Models.Enums;
using WebAPI.Models.Hero;

namespace WebAPI.Models.Player
{
    public static class GetPlayer
    {
        public sealed class Response
        {
            public Guid Id { get; set; }

            public Guid UserId { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public bool Displayed { get; set; }

            public PositionName Position { get; set; }

            public DateTime UpdatedAt { get; set; }

            public ICollection<GetHero.Response> Heroes { get; set; } = new HashSet<GetHero.Response>();
        }
    }
}
