using FluentResults;

namespace Library.Results.Successes.Message
{
    public class MessageSentSuccess : Success
    {
        public MessageSentSuccess() : this("Message has been sent")
        {
        }

        public MessageSentSuccess(string message) : base(message)
        {
            WithMetadata("key", nameof(MessageSentSuccess));
        }
    }

    public class InvitationSentSuccess : MessageSentSuccess
    {
        public InvitationSentSuccess(string message) : base(message)
        {
            WithMetadata("key", nameof(MessageSentSuccess));
        }

        public InvitationSentSuccess() : this("Team invitation has been sent")
        {
        }
    }

    public class ApplicationSentSuccess : MessageSentSuccess
    {
        public ApplicationSentSuccess(string message) : base(message)
        {
            WithMetadata("key", nameof(MessageSentSuccess));
        }

        public ApplicationSentSuccess() : this("Team application has been sent")
        {
        }
    }
}
