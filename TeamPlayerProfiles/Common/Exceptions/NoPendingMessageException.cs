using Library.Models.Enums;

namespace Common.Exceptions
{
    public class NoPendingMessageException(MessageType messageType) : Exception(messageType == MessageType.TeamApplication
        ? "There is no pending team application for this team" : "There is not pending player invitation for this player")
    {
    }
}
