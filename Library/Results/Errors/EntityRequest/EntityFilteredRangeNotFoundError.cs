namespace Library.Results.Errors.EntityRequest
{
    public class EntityFilteredRangeNotFoundError : EntityNotFoundError
    {
        public EntityFilteredRangeNotFoundError() : base("Entities matching given filtering query have not been been found") { }

        public EntityFilteredRangeNotFoundError(string customMessage) : base(customMessage) { }
    }
}
