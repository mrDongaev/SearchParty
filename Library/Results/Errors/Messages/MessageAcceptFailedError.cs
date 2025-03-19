namespace Library.Results.Errors.Messages
{
    public class MessageAcceptFailedError : ErrorWithData
    {
        public MessageAcceptFailedError(object? data = null) : this("Message could not be accepted", data) { }

        public MessageAcceptFailedError(string message, object? data = null) : base(message, data)
        {
            Metadata["ReasonName"] = nameof(MessageAcceptFailedError);
        }
    }
}
