using Common.Models;

namespace WebAPI.Contracts.Team
{
    public static class CreateTeam
    {
        public sealed class Request
        {
            public Guid UserId { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public ISet<PlayerInTeam> PlayersInTeam { get; set; }
        }
    }
}
