using FluentResults;

namespace Library.Results.Errors.EntityRequest
{
    public class EntityNotFoundError : Error
    {
        public EntityNotFoundError() : this("Entity has not been found")
        {
        }

        public EntityNotFoundError(string customMessage) : base(customMessage)
        {
            WithMetadata("key", nameof(EntityNotFoundError));
        }
    }
}
