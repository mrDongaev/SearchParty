namespace Library.Results.Errors.EntityRequest
{
    public class EntityRangeNotFoundError : EntityNotFoundError
    {

        public EntityRangeNotFoundError() : this("Entity range has not been found")
        {
        }

        public EntityRangeNotFoundError(string customMessage) : base(customMessage)
        {
            WithMetadata("key", nameof(EntityRangeNotFoundError));
        }
    }
}
