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
}
