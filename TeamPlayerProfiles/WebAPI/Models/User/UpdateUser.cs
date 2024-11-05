using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.User
{
    public static class UpdateUser
    {
        public sealed class Request
        {
            [Range(0, 20000, ErrorMessage = "MMR must be between 0 and 20000")]
            public uint? Mmr { get; set; }
        }
    }
}
