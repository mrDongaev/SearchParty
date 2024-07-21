using Common.Models;
using Common.Models.Enums;

namespace WebAPI.Contracts.Team
{
    public static class UpdateTeam
    {
        public sealed class Request
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public ISet<PlayerInTeam> Players { get; set; }
        }
    }
}
