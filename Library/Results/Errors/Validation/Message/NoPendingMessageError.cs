namespace Library.Results.Errors.Validation.Message
{
    public class NoPendingMessageError : ValidationError
    {
        public NoPendingMessageError() : this("There is no pending message for this team")
        {
        }

        public NoPendingMessageError(string message) : base(message)
        {
            WithMetadata("key", nameof(NoPendingMessageError));
        }
    }

    public class NoPendingInvitationError : NoPendingMessageError
    {
        public NoPendingInvitationError() : this("There is no pending player invitation for this team")
        {
        }

        public NoPendingInvitationError(string message) : base(message)
        {
            WithMetadata("key", nameof(NoPendingInvitationError));
        }
    }

    public class NoPendingApplicationError : NoPendingMessageError
    {
        public NoPendingApplicationError() : this($"There is no pending team application for this team")
        {
        }

        public NoPendingApplicationError(string message) : base(message)
        {
            WithMetadata("key", nameof(NoPendingApplicationError));
        }
    }
}
