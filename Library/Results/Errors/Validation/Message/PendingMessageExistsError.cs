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
}
