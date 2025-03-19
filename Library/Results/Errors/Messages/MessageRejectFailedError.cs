namespace Library.Results.Errors.Messages
{
    public class MessageRejectFailedError : ErrorWithData
    {
        public MessageRejectFailedError(object? data = null) : this("Message could not be rejected", data) { }

        public MessageRejectFailedError(string message, object? data = null) : base(message, data)
        {
            Metadata["ReasonName"] = nameof(MessageRejectFailedError);
        }
    }
}
