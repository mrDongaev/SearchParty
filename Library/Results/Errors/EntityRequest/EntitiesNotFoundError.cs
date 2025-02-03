namespace Library.Results.Errors.EntityRequest
{
    public class EntitiesNotFoundError : EntityNotFoundError
    {
        public EntitiesNotFoundError() : this("No entities have been found") { }

        public EntitiesNotFoundError(string customMessage) : base(customMessage)
        {
            WithMetadata("key", nameof(EntitiesNotFoundError));
        }
    }
}
