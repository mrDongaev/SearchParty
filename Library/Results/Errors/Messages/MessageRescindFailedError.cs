using FluentResults;

namespace Library.Results.Errors.Messages
{
    public class MessageRescindFailedError : Error
    {
        public MessageRescindFailedError() : this("Message could not be rescinded") { }

        public MessageRescindFailedError(string message) : base(message)
        {
            WithMetadata("key", nameof(MessageRescindFailedError));
        }
    }
}
