using FluentResults;
using Library.Models.Enums;

namespace Library.Results.Errors.Validation.Message
{
    public class SelfMessagingError : Error
    {
        public SelfMessagingError() : base("User cannot send a message to their own profile") { }

        public SelfMessagingError(MessageType type)
            : base($"User cannot send an {(type == MessageType.PlayerInvitation ? "invitation" : "application")} to their own {(type == MessageType.PlayerInvitation ? "player" : "team")} profile")
        {
        }
    }
}
