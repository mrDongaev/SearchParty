using System.ComponentModel.DataAnnotations;
using WebAPI.Validation;

namespace WebAPI.Models.Team
{
    public static class CreateTeam
    {
        public sealed class Request
        {
            [Required]
            public Guid? UserId { get; set; }

            [MaxLength(30)]
            public string? Name { get; set; } = "Профиль команды";

            [MaxLength(150)]
            public string? Description { get; set; }

            [MaxLength(5)]
            [UniqueTeamPositions]
            public ISet<UpdateTeamPlayer.Request>? PlayersInTeam { get; set; } = new HashSet<UpdateTeamPlayer.Request>();
        }
    }
}
