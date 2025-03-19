namespace Library.Results.Errors.EntityRequest
{
    public class EntitiesNotFoundError : EntityNotFoundError
    {
        public EntitiesNotFoundError(object? data = null) : this("No entities have been found", data) { }

        public EntitiesNotFoundError(string customMessage, object? data = null) : base(customMessage, data)
        {
            Metadata["ReasonName"] = nameof(EntitiesNotFoundError);
        }
    }
}
