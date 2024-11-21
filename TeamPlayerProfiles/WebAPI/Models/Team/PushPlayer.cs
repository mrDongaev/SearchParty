using Library.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Team
{
    public static class PushPlayer
    {
        public sealed class Request
        {
            public Guid? MessageId { get; set; }

            [Required]
            public Guid PlayerId { get; set; }

            [Required]
            public Guid TeamId { get; set; }

            [Required]
            public PositionName Position {  get; set; }

            public MessageType? MessageType { get; set; }
        }
    }
}
