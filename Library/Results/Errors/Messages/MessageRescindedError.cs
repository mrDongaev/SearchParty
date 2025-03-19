using FluentResults;

namespace Library.Results.Errors.Messages
{
    public class MessageRescindedError : ErrorWithData
    {
        public MessageRescindedError(object? data = null) : this("Message has already been rescinded", data) { }

        public MessageRescindedError(string message, object? data = null) : base(message, data)
        {
            WithMetadata("key", nameof(MessageRescindedError));
        }
    }
}
