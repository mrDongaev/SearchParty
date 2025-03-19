using FluentResults;

namespace Library.Results.Successes.Messages
{
    public class MessageRescindedSuccess : Success
    {
        public MessageRescindedSuccess() : base("Message has been successfully rescinded") { }

        public MessageRescindedSuccess(string message) : base(message)
        {
            Metadata["ReasonName"] = nameof(MessageRescindedSuccess);
        }
    }
}
