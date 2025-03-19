using FluentResults;

namespace Library.Results.Successes.Messages
{
    public class MessageAlreadyRescindedSuccess : Success
    {
        public MessageAlreadyRescindedSuccess() : this("Message has already been rescinded") { }

        public MessageAlreadyRescindedSuccess(string message) : base(message)
        {
            Metadata["ReasonName"] = nameof(MessageAlreadyRescindedSuccess);
        }
    }
}
