using FluentResults;

namespace Library.Results.Errors.EntityRequest
{
    public class EntityNotFoundError : Error
    {
        public EntityNotFoundError() : base("Entity has not been found")
        {
        }

        public EntityNotFoundError(string customMessage) : base(customMessage)
        {
        }
    }
}
