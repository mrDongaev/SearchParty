using FluentResults;

namespace Library.Results.Successes.Messages
{
    public class MessageAcceptedSuccess : Success
    {
        public MessageAcceptedSuccess() : base("Message has been successfully rescinded") { }

        public MessageAcceptedSuccess(string message) : base(message)
        {
            WithMetadata("key", nameof(MessageRescindedSuccess));
        }
    }
}
