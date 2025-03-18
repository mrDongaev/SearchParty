using FluentResults;

namespace Library.Results.Errors.Messages
{
    public class MessageAcceptedError : Error
    {
        public MessageAcceptedError() : this("Message has already been accepted") { }

        public MessageAcceptedError(string message) : base(message)
        {
            WithMetadata("key", nameof(MessageAcceptedError));
        }
    }
}
