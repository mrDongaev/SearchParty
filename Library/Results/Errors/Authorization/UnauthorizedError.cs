using FluentResults;

namespace Library.Results.Errors.Authorization
{
    public class UnauthorizedError : Error
    {
        public UnauthorizedError() : this("User is not authorized to perform this action") { }

        public UnauthorizedError(string customMessage) : base(customMessage)
        {
            WithMetadata("key", nameof(UnauthorizedError));
        }
    }
}
