using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.ExternalModels
{
    public class CreateUserInfoRequest
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public uint Mmr { get; set; }

        public string? SteamFriendCode { get; set; }

        public string? DiscordName { get; set; }

        public string? TelegramLink { get; set; }
    }
}
