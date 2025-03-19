namespace Library.Results.Errors.EntityRequest
{
    public class EntitiesForQueryNotFoundError : EntityNotFoundError
    {
        public EntitiesForQueryNotFoundError(object? data = null) : this("Entities matching given query have not been been found", data) { }

        public EntitiesForQueryNotFoundError(string customMessage, object? data = null) : base(customMessage, data)
        {
            WithMetadata("key", nameof(EntitiesForQueryNotFoundError));
        }
    }
}
