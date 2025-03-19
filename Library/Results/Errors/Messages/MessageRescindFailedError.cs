namespace Library.Results.Errors.Messages
{
    public class MessageRescindFailedError : ErrorWithData
    {
        public MessageRescindFailedError(object? data = null) : this("Message could not be rescinded", data) { }

        public MessageRescindFailedError(string message, object? data = null) : base(message, data)
        {
            Metadata["ReasonName"] = nameof(MessageRescindFailedError);
        }
    }
}
