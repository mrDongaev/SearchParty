using Common.Models;
using System.ComponentModel.DataAnnotations;
using WebAPI.Contracts.Team;

namespace WebAPI.Contracts.Team
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

            public ISet<TeamPlayerApi.Request>? PlayersInTeam { get; set; } = new HashSet<TeamPlayerApi.Request>();
        }
    }
}
