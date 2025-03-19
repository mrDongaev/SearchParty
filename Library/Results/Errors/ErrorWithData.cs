using FluentResults;
using Library.Results.Errors.Validation;

namespace Library.Results.Errors
{
    public class ErrorWithData : Error
    {
        public object? Data { get; protected set; } = null;

        public ErrorWithData(string message, object? data = null) : base(message)
        {
            Data = data;
            WithMetadata("key", nameof(ValidationError));
        }
    }
}
