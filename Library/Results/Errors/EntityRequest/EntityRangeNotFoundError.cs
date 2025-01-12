using FluentResults;

namespace Library.Results.Errors.EntityRequest
{
    public class EntityRangeNotFoundError : EntityNotFoundError
    {
        public EntityRangeNotFoundError() : base("Entity range has not been found")
        {
        }

        public EntityRangeNotFoundError(string customMessage) : base(customMessage)
        {
        }
    }
}
