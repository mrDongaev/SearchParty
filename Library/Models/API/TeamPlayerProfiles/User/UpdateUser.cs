using System.ComponentModel.DataAnnotations;

namespace Library.Models.API.TeamPlayerProfiles.User
{
    public static class UpdateUser
    {
        public sealed class Request
        {
            public Guid Id { get; set; }

            public uint? Mmr { get; set; }
        }
    }
}
