using FluentResults;

namespace Library.Results.Errors.Messages
{
    public class MessageExpiredError : ErrorWithData
    {
        public MessageExpiredError(object? data = null) : this("Message has expired", data) { }

        public MessageExpiredError(string message, object? data = null) : base(message, data)
        {
            WithMetadata("key", nameof(MessageExpiredError));
        }
    }
}
