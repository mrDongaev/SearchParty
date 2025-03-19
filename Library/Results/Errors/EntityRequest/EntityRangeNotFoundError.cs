namespace Library.Results.Errors.EntityRequest
{
    public class EntityRangeNotFoundError : EntityNotFoundError
    {

        public EntityRangeNotFoundError(object? data = null) : this("Entity range has not been found", data)
        {
        }

        public EntityRangeNotFoundError(string customMessage, object? data = null) : base(customMessage, data)
        {
            WithMetadata("key", nameof(EntityRangeNotFoundError));
        }
    }
}
