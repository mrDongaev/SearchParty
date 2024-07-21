using Service.Contracts.Hero;
using WebAPI.Contracts.Hero;
using Common.Models.Enums;

namespace WebAPI.Contracts.Player
{
    public static class CreatePlayer
    {
        public sealed class Request
        {
            public Guid UserId { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public PositionName Position { get; set; }

            public ISet<int> Heroes { get; set; }
        }
    }
}
