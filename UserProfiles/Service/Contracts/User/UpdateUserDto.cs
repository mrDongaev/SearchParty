﻿namespace Service.Contracts.User
{
    public class UpdateUserDto
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? SteamFriendCode { get; set; }

        public string? DiscordName { get; set; }

        public string? TelegramLink { get; set; }
    }
}
