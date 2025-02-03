using FluentResults;

namespace Library.Results.Errors.Validation
{
    public class ValidationError : Error
    {
        public ValidationError() : this("A validation error has occured") { }

        public ValidationError(string message) : base(message)
        {
            WithMetadata("key", nameof(ValidationError));
        }
    }
}
