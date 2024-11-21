using Library.Models.Enums;

namespace Common.Exceptions
{
    public class PendingMessageExistsException(MessageType messageType) : Exception(messageType == MessageType.TeamApplication
        ? "A pending application already exists for these team and player profiles" : "A pending inviation already exists for these team and player profiles")
    {
    }
}
