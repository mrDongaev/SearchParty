namespace Library.Results.Errors.EntityRequest
{
    public class EntityAlreadyExistsError : ErrorWithData
    {
        public EntityAlreadyExistsError(object? data = null) : this("Entity with the given ID already exists", data) { }

        public EntityAlreadyExistsError(string message, object? data = null) : base(message, data)
        {
            Metadata["ReasonName"] = nameof(EntityAlreadyExistsError);
        }
    }
}
