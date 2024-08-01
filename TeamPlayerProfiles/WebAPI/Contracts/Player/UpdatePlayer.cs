using Common.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Contracts.Player
{
    public static class UpdatePlayer
    {
        public sealed class Request
        {
            [Required]
            public Guid? Id { get; set; }

            [MaxLength(30)]
            public string? Name { get; set; }

            [MaxLength(150)]
            public string? Description { get; set; }

            public PositionName? Position { get; set; }
        }
    }
}
