using FluentResults;

namespace Library.Results.Successes.Messages
{
    public class MessageSentSuccess : Success
    {
        public MessageSentSuccess() : this("Message has been sent successfully")
        {
        }

        public MessageSentSuccess(string message) : base(message)
        {
            Metadata["ReasonName"] = nameof(MessageSentSuccess);
        }
    }
}
