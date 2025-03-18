using FluentResults;

namespace Library.Results.Errors.Messages
{
    public class MessageAcceptFailedError : Error
    {
        public MessageAcceptFailedError() : this("Message could not be accepted") { }

        public MessageAcceptFailedError(string message) : base(message)
        {
            WithMetadata("key", nameof(MessageAcceptFailedError));
        }
    }
}
