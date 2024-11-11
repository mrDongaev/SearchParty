namespace Library.Models.API.UserInfo.User
{
    public static class UpdateUser
    {
        public sealed class Request
        {
            public string? Name { get; set; }

            public string? Description { get; set; }

            public uint? Mmr { get; set; }

            public string? SteamFriendCode { get; set; }

            public string? DiscordName { get; set; }

            public string? TelegramLink { get; set; }
        }
    }
}
