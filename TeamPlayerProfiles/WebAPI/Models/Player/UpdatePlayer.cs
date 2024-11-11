using Library.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Player
{
    public static class UpdatePlayer
    {
        public sealed class Request
        {
            [MaxLength(30)]
            public string? Name { get; set; }

            [MaxLength(150)]
            public string? Description { get; set; }

            public PositionName? Position { get; set; }

            public ISet<int>? HeroIds { get; set; }
        }
    }
}
