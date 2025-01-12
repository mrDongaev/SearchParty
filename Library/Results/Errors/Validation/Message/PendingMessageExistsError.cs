using Library.Models.Enums;

namespace Library.Results.Errors.Validation.Message
{
    public class PendingMessageExistsError : ValidationError
    {
        public PendingMessageExistsError(MessageType type) : base($"A pending {(type == MessageType.PlayerInvitation ? "invitation" : "application")} already exists for these team and player profiles")
        {
        }
    }
}
