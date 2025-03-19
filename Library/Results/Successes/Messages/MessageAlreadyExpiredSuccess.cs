using FluentResults;

namespace Library.Results.Successes.Messages
{
    public class MessageAlreadyExpiredSuccess : Success
    {
        public MessageAlreadyExpiredSuccess() : this("Message has expired") { }

        public MessageAlreadyExpiredSuccess(string message) : base(message)
        {
            Metadata["ReasonName"] = nameof(MessageAlreadyExpiredSuccess);
        }
    }
}
