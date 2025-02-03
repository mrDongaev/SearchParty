namespace Library.Results.Errors.EntityRequest
{
    public class EntitiesForQueryNotFoundError : EntityNotFoundError
    {
        public EntitiesForQueryNotFoundError() : this("Entities matching given query have not been been found") { }

        public EntitiesForQueryNotFoundError(string customMessage) : base(customMessage)
        {
            WithMetadata("key", nameof(EntitiesForQueryNotFoundError));
        }
    }
}
