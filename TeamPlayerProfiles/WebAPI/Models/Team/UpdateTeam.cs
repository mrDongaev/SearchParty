using System.ComponentModel.DataAnnotations;

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
        }
    }
}
