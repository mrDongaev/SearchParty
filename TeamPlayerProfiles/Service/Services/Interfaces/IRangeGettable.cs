namespace Service.Services.Interfaces
{
    public interface IRangeGettable<TGetDto, TId>
    {
        /// <summary>
        /// Получить ряд сущностей по идентификатору
        /// </summary>
        /// <param name="ids">Список идентификаторов</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Список ДТО сущностей</returns>
        Task<ICollection<TGetDto>> GetRange(ICollection<TId> ids, CancellationToken cancellationToken);
    }
}
