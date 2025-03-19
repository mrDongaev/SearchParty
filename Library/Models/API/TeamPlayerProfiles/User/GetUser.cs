namespace Library.Models.API.TeamPlayerProfiles.User
{
    public static class GetUser
    {
        public sealed class Response
        {
            public Guid Id { get; set; }

            public uint Mmr { get; set; }

            public DateTime UpdatedAt { get; set; }
        }
    }
}
