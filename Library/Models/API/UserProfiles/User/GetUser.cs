namespace Library.Models.API.UserProfiles.User
{
    public static class GetUser
    {
        public sealed class Response
        {
            public Guid Id { get; set; }

            public uint Mmr { get; set; }
        }
    }
}
