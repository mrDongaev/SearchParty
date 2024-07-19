namespace DataAccess.Repositories.Interfaces
{
    public interface IRangeGettable<T, TId>
    {
        Task<ICollection<T>> GetRange(ICollection<TId> ids, CancellationToken cancellationToken);
    }
}
