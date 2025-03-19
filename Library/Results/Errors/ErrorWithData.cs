using FluentResults;

namespace Library.Results.Errors
{
    public class ErrorWithData : Error
    {
        public object? Data { get; set; } = null;

        public ErrorWithData(string message, object? data = null) : base(message)
        {
            Data = data;
            Metadata["ReasonName"] = nameof(ErrorWithData);
        }
    }
}
