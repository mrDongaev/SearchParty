using FluentResults;

namespace Library.Results.Errors.Messages
{
    public class MessageRejectedError : Error
    {
        public MessageRejectedError() : this("Message has already been rejected") { }

        public MessageRejectedError(string message) : base(message)
        {
            WithMetadata("key", nameof(MessageRejectedError));
        }
    }
}
