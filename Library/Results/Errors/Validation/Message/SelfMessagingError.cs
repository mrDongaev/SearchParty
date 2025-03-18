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
}
