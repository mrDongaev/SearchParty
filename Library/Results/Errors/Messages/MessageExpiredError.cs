using FluentResults;

namespace Library.Results.Errors.Messages
{
    public class MessageExpiredError : Error
    {
        public MessageExpiredError() : this("Message has expired") { }

        public MessageExpiredError(string message) : base(message)
        {
            WithMetadata("key", nameof(MessageExpiredError));
        }
    }
}
