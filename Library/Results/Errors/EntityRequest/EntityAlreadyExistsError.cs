using FluentResults;

namespace Library.Results.Errors.EntityRequest
{
    public class EntityAlreadyExistsError : Error
    {
        public EntityAlreadyExistsError() : this("Entity with the given ID already exists") { }

        public EntityAlreadyExistsError(string message) : base(message)
        {
            WithMetadata("key", nameof(EntityAlreadyExistsError));
        }
    }
}
