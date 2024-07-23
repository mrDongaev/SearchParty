using Common.Models;

namespace WebAPI.Contracts.Team
{
    public static class GetTeam
    {
        public sealed class Response
        {
            public Guid Id { get; set; }

            public Guid UserId { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public bool Displayed { get; set; }

            public DateTime UpdatedAt { get; set; }

            public ISet<PlayerInTeam> Players { get; set; }
        }
    }
}
