using DataAccess.Entities.Interfaces;

namespace DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Описания общих методов для всех репозиториев.
    /// </summary>
    /// <typeparam name="T"> Тип Entity для репозитория. </typeparam>
    /// <typeparam name="TId"> Тип первичного ключа. </typeparam>
    public interface IRepository<T, TId>
        where T : IEntity<TId>
    {
        /// <summary>
        /// Запросить все сущности в базе.
        /// </summary>
        /// <param name="cancellationToken"> Токен отмены. </param>
        /// <returns> Список сущностей. </returns>
        Task<ICollection<T>> GetAll(CancellationToken cancellationToken);

        /// <summary>
        /// Получить сущность по Id.
        /// </summary>
        /// <param name="id"> Id сущности. </param>
        /// <param name="cancellationToken"> Токен отмены </param>
        /// <returns> Cущность. </returns>
        Task<T> Get(TId id, CancellationToken cancellationToken);

        /// <summary>
        /// Удалить сущность.
        /// </summary>
        /// <param name="id"> Id удалённой сущности. </param>
        /// <param name="cancellationToken"> Токен отмены </param>
        /// <returns> Была ли сущность удалена. </returns>
        Task<bool> Delete(TId id, CancellationToken cancellationToken);

        /// <summary>
        /// Удалить сущности.
        /// </summary>
        /// <param name="entities"> Коллекция сущностей для удаления. </param>
        /// <param name="cancellationToken"> Токен отмены </param>
        /// <returns> Была ли операция удаления успешна. </returns>
        Task<bool> DeleteRange(ICollection<T> entities, CancellationToken cancellationToken);

        /// <summary>
        /// Для сущности проставить состояние - что она изменена.
        /// </summary>
        /// <param name="entity"> Сущность для изменения. </param>
        /// <param name="cancellationToken"> Токен отмены </param>
        Task<T> Update(T entity, CancellationToken cancellationToken);

        /// <summary>
        /// Добавить в базу одну сущность.
        /// </summary>
        /// <param name="entity"> Сущность для добавления. </param>
        /// <param name="cancellationToken"> Токен отмены </param>
        /// <returns> Добавленная сущность. </returns>
        Task<T> Add(T entity, CancellationToken cancellationToken);

        /// <summary>
        /// Добавить в базу массив сущностей.
        /// </summary>
        /// <param name="entities"> Массив сущностей. </param>
        /// <param name="cancellationToken"> Токен отмены </param>
        Task<bool> AddRange(ICollection<T> entities, CancellationToken cancellationToken);
    }
}
