using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.User
{
    public static class UpdateUser
    {
        public sealed class Request
        {
            [Required]
            public Guid Id { get; set; }

            [MaxLength(30)]
            public string? Name { get; set; }

            [MaxLength(150)]
            public string? Description { get; set; }

            [Range(0, 20000, ErrorMessage = "MMR must be between 0 and 20000")]
            public uint? Mmr { get; set; }

            [Length(8, 8)]
            [RegularExpression(@"^\d{8}$", ErrorMessage = "Invalid Steam friend code")]
            public string? SteamFriendCode { get; set; }

            [Length(2, 32)]
            [RegularExpression(@"^[^#]{2,32}#\d{4}$|^(?!.*\.\.|[A-Z])[a-z\d_.]{2,32}$", ErrorMessage = "Invalid Discord name")]
            public string? DiscordName { get; set; }

            [RegularExpression(@"^(?:@|(?:(?:(?:https?://)?t(?:elegram)?)\.me\/))(\w{4,})$", ErrorMessage = "Invalid Telegram username")]
            public string? TelegramLink { get; set; }
        }
    }
}
