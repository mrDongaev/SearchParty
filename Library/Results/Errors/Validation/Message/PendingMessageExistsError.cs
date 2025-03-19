namespace Library.Results.Errors.Validation.Message
{
    public class PendingMessageExistsError : ValidationError
    {
        public PendingMessageExistsError(object? data = null) : this("A pending message already exists for these team and player profiles", data)
        {
        }

        public PendingMessageExistsError(string message, object? data = null) : base(message, data)
        {
            Metadata["ReasonName"] = nameof(PendingMessageExistsError);
        }
    }
}
