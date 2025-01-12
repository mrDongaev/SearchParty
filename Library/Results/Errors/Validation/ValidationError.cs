using FluentResults;

namespace Library.Results.Errors.Validation
{
    public class ValidationError : Error
    {
        public ValidationError() : base("A validation error has occured") { }

        public ValidationError(string message) : base(message) { }
    }
}
