using FluentResults;

namespace Library.Results.Errors.Messages
{
    public class MessageRejectedError : ErrorWithData
    {
        public MessageRejectedError(object? data = null) : this("Message has already been rejected", object ? data = null) { }

        public MessageRejectedError(string message, object? data = null) : base(message, data)
        {
            WithMetadata("key", nameof(MessageRejectedError));
        }
    }
}
