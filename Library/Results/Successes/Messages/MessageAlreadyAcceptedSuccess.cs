using FluentResults;

namespace Library.Results.Successes.Messages
{
    public class MessageAlreadyAcceptedSuccess : Success
    {
        public MessageAlreadyAcceptedSuccess() : this("Message has already been accepted") { }

        public MessageAlreadyAcceptedSuccess(string message) : base(message)
        {
            Metadata["ReasonName"] = nameof(MessageAlreadyAcceptedSuccess);
        }
    }
}
