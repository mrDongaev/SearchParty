using FluentResults;

namespace Library.Results.Errors.Authorization
{
    public class UnauthorizedError : ErrorWithData
    {
        public UnauthorizedError(object? data = null) : this("User is not authorized to perform this action", data) { }

        public UnauthorizedError(string customMessage, object? data = null) : base(customMessage, data)
        {
            WithMetadata("key", nameof(UnauthorizedError));
        }
    }
}
