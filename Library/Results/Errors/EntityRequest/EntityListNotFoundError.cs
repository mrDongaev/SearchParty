namespace Library.Results.Errors.EntityRequest
{
    public class EntityListNotFoundError : EntityNotFoundError
    {
        public EntityListNotFoundError() : base("No entities have been found") { }

        public EntityListNotFoundError(string customMessage) : base(customMessage) { }
    }
}
