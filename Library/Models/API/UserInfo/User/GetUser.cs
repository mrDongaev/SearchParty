﻿namespace Library.Models.API.UserInfo.User
{
    public static class GetUser
    {
        public sealed class Response
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public uint Mmr { get; set; }

            public string SteamFriendCode { get; set; }

            public string DiscordName { get; set; }

            public string TelegramLink { get; set; }

            public DateTime UpdatedAt { get; set; }
        }
    }
}
