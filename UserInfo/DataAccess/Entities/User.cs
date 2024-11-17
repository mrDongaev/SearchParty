using Library.Entities.Interfaces;

namespace DataAccess.Entities
{
    public class User : IEntity<Guid>, IUpdateable
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? SteamFriendCode { get; set; }

        public string? DiscordName { get; set; }

        public string? TelegramLink { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
