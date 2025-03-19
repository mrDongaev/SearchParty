namespace Library.Results.Errors.Validation.Message
{
    public class NoPendingMessageError : ValidationError
    {
        public NoPendingMessageError(object? data = null) : this("There is no pending message for this team", data)
        {
        }

        public NoPendingMessageError(string message, object? data = null) : base(message, data)
        {
            WithMetadata("key", nameof(NoPendingMessageError));
        }
    }
}
