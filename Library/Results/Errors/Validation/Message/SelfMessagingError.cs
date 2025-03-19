namespace Library.Results.Errors.Validation.Message
{
    public class SelfMessagingError : ValidationError
    {
        public SelfMessagingError(object? data = null) : this("User cannot send a message to their own profile", data)
        {
        }

        public SelfMessagingError(string message, object? data = null) : base(message, data)
        {
            WithMetadata("key", nameof(SelfMessagingError));
        }
    }
}
