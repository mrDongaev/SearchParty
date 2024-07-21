using Common.Models.Enums;
using Service.Contracts.Hero;

namespace WebAPI.Contracts.Player
{
    public static class UpdatePlayer
    {
        public sealed class Request
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public PositionName Position { get; set; }

            public ISet<int> Heroes { get; set; }
        }
    }
}
