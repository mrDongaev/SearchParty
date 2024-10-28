using Library.Entities.Interfaces;

namespace DataAccess.Entities
{
    public class User : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public uint Mmr { get; set; } = 0;

        public string? SteamFriendCode { get; set; }

        public string? DiscordName { get; set; }

        public string? TelegramLink { get; set; }
    }
}
