using FluentResults;

namespace Library.Results.Successes.Messages
{
    public class MessageAlreadyRejectedSuccess : Success
    {
        public MessageAlreadyRejectedSuccess() : this("Message has already been rejected") { }

        public MessageAlreadyRejectedSuccess(string message) : base(message)
        {
            Metadata["ReasonName"] = nameof(MessageAlreadyRejectedSuccess);
        }
    }
}
