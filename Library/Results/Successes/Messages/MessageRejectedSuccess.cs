using FluentResults;

namespace Library.Results.Successes.Messages
{
    public class MessageRejectedSuccess : Success
    {
        public MessageRejectedSuccess() : base("Message has been successfully rescinded") { }

        public MessageRejectedSuccess(string message) : base(message)
        {
            WithMetadata("key", nameof(MessageRescindedSuccess));
        }
    }
}
