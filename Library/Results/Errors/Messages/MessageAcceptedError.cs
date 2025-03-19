using FluentResults;

namespace Library.Results.Errors.Messages
{
    public class MessageAcceptedError : ErrorWithData
    {
        public MessageAcceptedError(object? data = null) : this("Message has already been accepted", data) { }

        public MessageAcceptedError(string message, object? data = null) : base(message, data)
        {
            WithMetadata("key", nameof(MessageAcceptedError));
        }
    }
}
