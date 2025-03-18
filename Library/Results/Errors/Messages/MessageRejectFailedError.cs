using FluentResults;

namespace Library.Results.Errors.Messages
{
    public class MessageRejectFailedError : Error
    {
        public MessageRejectFailedError() : this("Message could not be rejected") { }

        public MessageRejectFailedError(string message) : base(message)
        {
            WithMetadata("key", nameof(MessageRejectFailedError));
        }
    }
}
