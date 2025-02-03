using FluentResults;
using Library.Models.Enums;

namespace Library.Results.Errors.Validation.Message
{
    public class SelfMessagingError : ValidationError
    {
        public SelfMessagingError() : this("User cannot send a message to their own profile")
        {
        }

        public SelfMessagingError(string message) : base(message)
        {
            WithMetadata("key", nameof(SelfMessagingError));
        }
    }

    public class SelfInvitationError : SelfMessagingError
    {
        public SelfInvitationError() : this("User cannot send an invitation to their own player profile")
        {
        }

        public SelfInvitationError(string message) : base(message)
        {
            WithMetadata("key", nameof(SelfInvitationError));
        }
    }

    public class SelfApplicationError : SelfMessagingError
    {
        public SelfApplicationError() : this("User cannot send an application to their own team profile")
        {
        }

        public SelfApplicationError(string message) : base(message)
        {
            WithMetadata("key", nameof(SelfApplicationError));
        }
    }
}
