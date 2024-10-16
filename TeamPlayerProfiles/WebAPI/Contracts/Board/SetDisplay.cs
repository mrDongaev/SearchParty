using System.ComponentModel.DataAnnotations;

namespace WebAPI.Contracts.Board
{
    public static class SetDisplay
    {
        public sealed class Request()
        {
            [Required]
            public bool? Displayed { get; set; }
        }
    }
}
