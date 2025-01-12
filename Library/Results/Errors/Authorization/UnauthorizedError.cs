using FluentResults;

namespace Library.Results.Errors.Authorization
{
    public class UnauthorizedError : Error
    {
        public UnauthorizedError() : base("User is not authorized to perform this action") { }

        public UnauthorizedError(string customMessage) : base(customMessage) { }
    }
}
