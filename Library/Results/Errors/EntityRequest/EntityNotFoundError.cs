namespace Library.Results.Errors.EntityRequest
{
    public class EntityNotFoundError : ErrorWithData
    {
        public EntityNotFoundError(object? data = null) : this("Entity has not been found", data)
        {
        }

        public EntityNotFoundError(string customMessage, object? data = null) : base(customMessage, data)
        {
            Metadata["ReasonName"] = nameof(EntityNotFoundError);
        }
    }
}
