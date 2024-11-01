using Library.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Player
{
    public static class CreatePlayer
    {
        public sealed class Request
        {
            [Required]
            public Guid? UserId { get; set; }

            [MaxLength(30)]
            public string? Name { get; set; } = "Профиль игрока";

            [MaxLength(150)]
            public string? Description { get; set; }

            [Required]
            public PositionName? Position { get; set; } = PositionName.Carry;

            public ISet<int>? HeroIds { get; set; } = new HashSet<int>();
        }
    }
}
