namespace Library.Models.API.UserProfiles.User
{
    public static class CreateUser
    {
        public sealed class Request
        {
            public Guid Id { get; set; }

            public uint Mmr { get; set; }
        }
    }
}
