namespace DataAccess.Entities.Interfaces
{
    public interface IEntity<TId>
    {
        TId Id { get; set; }
    }
}
