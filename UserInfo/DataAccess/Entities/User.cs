using Library.Entities.Interfaces;

namespace DataAccess.Entities
{
    public class User : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? SteamFriendCode { get; set; }

        public string? DiscordName { get; set; }

        public string? TelegramLink { get; set; }
    }
}
