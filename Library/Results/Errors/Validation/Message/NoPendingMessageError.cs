using Library.Models.Enums;

namespace Library.Results.Errors.Validation.Message
{
    public class NoPendingMessageError : ValidationError
    {
        public NoPendingMessageError(MessageType type) : base($"There is no pending {(type == MessageType.PlayerInvitation ? "player invitation" : "team application")} for this team") { }
    }
}
