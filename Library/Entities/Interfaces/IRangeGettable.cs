namespace Library.Entities.Interfaces
{
    public interface IRangeGettable<T, TId>
    {
        /// <summary>
        /// Получить ряд сущностей по идентификатору
        /// </summary>
        /// <param name="ids">Список идентификаторов</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Список ДТО сущностей</returns>
        Task<ICollection<T>> GetRange(ICollection<TId> ids, CancellationToken cancellationToken);
    }
}
