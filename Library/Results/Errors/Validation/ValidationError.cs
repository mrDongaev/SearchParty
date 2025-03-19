using FluentResults;

namespace Library.Results.Errors.Validation
{
    public class ValidationError : ErrorWithData
    {
        public ValidationError(object? data = null) : this("A validation error has occured", data) { }

        public ValidationError(string message, object? data = null) : base(message, data)
        {
            WithMetadata("key", nameof(ValidationError));
        }
    }
}
