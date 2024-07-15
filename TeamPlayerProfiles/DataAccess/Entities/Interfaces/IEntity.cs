namespace DataAccess.Entities.Interfaces
{
    public interface IEntity<TId>
    {
        public TId Id { get; set; }
    }
}
