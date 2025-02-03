using Library.Models.Enums;
using System.Diagnostics.Contracts;

namespace Library.Results.Errors.Validation.Message
{
    public class PendingMessageExistsError : ValidationError
    {
        public PendingMessageExistsError() : this("A pending message already exists for these team and player profiles")
        {
        }

        public PendingMessageExistsError(string message) : base(message)
        {
            WithMetadata("key", nameof(PendingMessageExistsError));
        }
    }

    public class PendingInvitationExistsError : PendingMessageExistsError
    {
        public PendingInvitationExistsError() : this("A pending invitation already exists for these team and player profiles")
        {
        }

        public PendingInvitationExistsError(string message) : base(message)
        {
            WithMetadata("key", nameof(PendingInvitationExistsError));
        }
    }

    public class PendingApplicationExistsError : PendingMessageExistsError
    {
        public PendingApplicationExistsError() : this("A pending application already exists for these team and player profiles")
        {
        }

        public PendingApplicationExistsError(string message) : base(message)
        {
            WithMetadata("key", nameof(PendingApplicationExistsError));
        }
    }
}
