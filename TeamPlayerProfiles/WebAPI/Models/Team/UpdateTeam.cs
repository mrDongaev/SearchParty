using System.ComponentModel.DataAnnotations;
using WebAPI.Validation;

namespace WebAPI.Models.Team
{
    public static class UpdateTeam
    {
        public sealed class Request
        {
            [MaxLength(30)]
            public string? Name { get; set; }

            [MaxLength(150)]
            public string? Description { get; set; }

            [MaxLength(5)]
            [UniqueTeamPositions]
            public ISet<UpdateTeamPlayer.Request>? PlayersInTeam { get; set; }
        }
    }
}
