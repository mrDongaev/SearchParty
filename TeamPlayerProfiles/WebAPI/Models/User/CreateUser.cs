using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.User
{
    public static class CreateUser
    {
        public sealed class Request
        {
            [Required]
            public Guid Id { get; set; }

            [Required]
            [Range(0, 20000, ErrorMessage = "MMR must be between 0 and 20000")]
            public uint Mmr { get; set; }
        }
    }
}
