using FluentResults;

namespace Library.Results.Errors.Messages
{
    public class MessageRescindedError : Error
    {
        public MessageRescindedError() : this("Message has already been rescinded") { }

        public MessageRescindedError(string message) : base(message)
        {
            WithMetadata("key", nameof(MessageRescindedError));
        }
    }
}
